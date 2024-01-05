namespace Product.Core.Domain.ExpressionBuilder
{
    public class ExpressionParameter
    {
        public ExpressionOperator Operator { get; set; }

        public string PropertyName { get; set; }

        public string Value { get; set; }
    }
}
