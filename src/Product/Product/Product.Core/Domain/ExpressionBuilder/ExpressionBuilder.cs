using Product.Core.Domain.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Product.Core.Domain.ExpressionBuilder
{
    public static class ExpressionBuilder
    {
        private static object ConvertToPropType(PropertyInfo property, object value)
        {
            object obj2 = null;
            if (property == null)
            {
                return obj2;
            }
            Type underlyingType = Nullable.GetUnderlyingType(property.PropertyType);
            bool flag = underlyingType != null;
            if (!flag)
            {
                underlyingType = property.PropertyType;
            }
            bool flag2 = (value != null) || flag;
            if (!flag2)
            {
                throw new Exception("Cant attrib null on non nullable. ");
            }
            if (underlyingType.IsEnum)
            {
                return (((value == null) || Convert.IsDBNull(value)) ? null : Enum.Parse(underlyingType, value.ToString()));
            }
            return (((value == null) || Convert.IsDBNull(value)) ? null : Convert.ChangeType(value, underlyingType));
        }
        private static string MakeSureFirtCapitalLetterIsUpper(this string @string)
        {
            var result = @string.First().ToString().ToUpper() + @string.Substring(1);
            return result;
        }

        private static Expression GetExpression<TEntity>(ParameterExpression parameterExpression, ExpressionParameter parameter)
        {
            var propertyName = parameter.PropertyName.MakeSureFirtCapitalLetterIsUpper();

            PropertyInfo property = typeof(TEntity).GetProperty(propertyName);
            object obj2 = ConvertToPropType(property, parameter.Value);
            MemberExpression left = Expression.Property(parameterExpression, property);
            ConstantExpression right = Expression.Constant(obj2, property.PropertyType);
            switch (parameter.Operator)
            {
                case ExpressionOperator.Equals:
                    return Expression.Equal(left, right);

                case ExpressionOperator.NotEquals:
                    return Expression.NotEqual(left, right);

                case ExpressionOperator.GreaterThan:
                    return Expression.GreaterThan(left, right);

                case ExpressionOperator.LessThan:
                    return Expression.LessThan(left, right);

                case ExpressionOperator.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(left, right);

                case ExpressionOperator.LessThanOrEqual:
                    return Expression.LessThanOrEqual(left, right);

                case ExpressionOperator.Contains:
                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    return Expression.Call(left, containsMethod, new Expression[] { right });

                case ExpressionOperator.StartsWith:
                    var startsWith = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                    return Expression.Call(left, startsWith, new Expression[] { right });

                case ExpressionOperator.EndsWith:
                    var endsWith = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                    return Expression.Call(left, endsWith, new Expression[] { right });
            }
            return null;
        }

        public static Expression<Func<TEntity, bool>> ToExpression<TEntity>(this List<ExpressionParameter> parameter) //where TEntity : class
        {
            Expression<Func<TEntity, bool>> expression = null;
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TEntity), "x");
            if ((parameter == null) || (parameter.Count == 0))
            {
                return expression;
            }
            Expression left = null;
            foreach (ExpressionParameter expressionParameter in parameter)
            {
                if (left == null)
                {
                    left = GetExpression<TEntity>(parameterExpression = Expression.Parameter(typeof(TEntity), "x"), expressionParameter);
                }
                else
                {
                    left = Expression.AndAlso(left, GetExpression<TEntity>(parameterExpression, expressionParameter));
                }
            }
            return Expression.Lambda<Func<TEntity, bool>>(left, new ParameterExpression[] { parameterExpression });
        }

        public static IQueryable<TEntity> ToOrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty, OrderByType orderByType)
        {
            string methodName = (orderByType == OrderByType.Desc) ? "OrderByDescending" : "OrderBy";

            Type type = typeof(TEntity);

            PropertyInfo property = type.GetProperty(orderByProperty.MakeSureFirtCapitalLetterIsUpper());

            if (property == null)
            {
                throw new Exception(String.Format("This Property: {0} Not a Member Class Name: {1}", orderByProperty, type.FullName));
            }

            ParameterExpression expression = Expression.Parameter(type, "x");

            LambdaExpression expression3 = Expression.Lambda(Expression.MakeMemberAccess(expression, property), new ParameterExpression[] { expression });

            MethodCallExpression expression4 = Expression.Call(typeof(Queryable), methodName, new Type[] { type, property.PropertyType }, new Expression[] { source.Expression, Expression.Quote(expression3) });

            return source.Provider.CreateQuery<TEntity>(expression4);
        }
    }
}
