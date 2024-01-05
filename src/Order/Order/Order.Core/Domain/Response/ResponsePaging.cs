using Order.Core.Domain.ExpressionBuilder;
using Order.Core.Domain.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Order.Core.Domain.Response
{
    [Serializable]
    public class ResponsePaging
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;

        public bool HasNext => CurrentPage < TotalPages;

        public ResponsePaging() { }

        public static ResponsePaging Create() => new ResponsePaging();

        public ResponsePaging(
            int totalCount,
            int pageNumber,
            int pageSize)
        {
            TotalCount = totalCount;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        public static ResponsePaging Create(int totalCount, int pageNumber, int pageSize)
            => new ResponsePaging(totalCount, pageNumber, pageSize);
    }

    [Serializable]
    public class ResponsePaging<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;

        public bool HasNext => CurrentPage < TotalPages;

        public ResponsePaging(
            List<T> items,
            int count,
            int pageNumber,
            int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public static ResponsePaging<T> Create(
            List<T> items,
            int count,
            int pageNumber,
            int pageSize)
        {
            return new ResponsePaging<T>(items, count, pageNumber, pageSize);
        }

        public static ResponsePaging<T> Create(
            IQueryable<T> source,
            RequestPaging paging)
        {
            return Create(
                source,
                paging.SkipCount + 1,
                paging.TakeCount,
                paging.FilterParameter,
                paging.OrderColumn,
                paging.OrderByType);
        }

        public static async Task<ResponsePaging<T>> CreateAsync(
            IQueryable<T> source,
            RequestPaging paging)
        {
            return
                await CreateAsync(
                    source,
                    paging.SkipCount + 1,
                    paging.TakeCount,
                    paging.FilterParameter,
                    paging.OrderColumn,
                    paging.OrderByType);
        }


        public static ResponsePaging<T> Create(
              IQueryable<T> source,
              int pageNumber,
              int pageSize,
              List<ExpressionParameter> filterParameter,
              string orderBy,
              OrderByType? orderByType)
        {
            if (orderByType == null)
            {
                orderByType = OrderByType.Asc;
            }

            Expression<Func<T, bool>> predicate = null;
            if (filterParameter != null && filterParameter.Count > 0)
            {
                predicate = filterParameter.ToExpression<T>();
                source = source.Where<T>(predicate).AsQueryable<T>();
            }

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                source = source.ToOrderBy<T>(orderBy, (OrderByType)orderByType);
            }

            var count = source.Count();
            var skip = (pageNumber - 1) * pageSize;
            var items = source.Skip(skip).Take(pageSize).ToList();

            return new ResponsePaging<T>(items, count, pageNumber, pageSize);
        }

        public static async Task<ResponsePaging<T>> CreateAsync(
              IQueryable<T> source,
              int pageNumber,
              int pageSize,
              List<ExpressionParameter> filterParameter,
              string orderBy,
              OrderByType? orderByType)
        {
            if (orderByType == null)
            {
                orderByType = OrderByType.Asc;
            }

            Expression<Func<T, bool>> predicate = null;
            if (filterParameter != null && filterParameter.Count > 0)
            {
                predicate = filterParameter.ToExpression<T>();
                source = source.Where<T>(predicate).AsQueryable<T>();
            }

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                source = source.ToOrderBy<T>(orderBy, (OrderByType)orderByType);
            }

            var count = source.Count();
            var skip = (pageNumber - 1) * pageSize;
            var items = await Task.FromResult(source.Skip(skip).Take(pageSize).ToList());

            return new ResponsePaging<T>(items, count, pageNumber, pageSize);
        }
    }

    public static class ResponsePagingExtensions
    {
        public static ResponsePaging<T> GetPaged<T>(
            this IQueryable<T> query,
            RequestPaging paging)
        {
            var response =
                ResponsePaging<T>.Create(query, paging);

            return response;
        }

        public static ResponsePaging<T> GetPaged<T>(
            this IQueryable<T> query,
            int pageNumber,
            int pageSize,
            List<ExpressionParameter> filterParameter = null,
            string orderBy = null,
            OrderByType? orderByType = null)
        {
            var response =
                ResponsePaging<T>.Create(query, pageNumber, pageSize, filterParameter, orderBy, orderByType);

            return response;
        }

        public static async Task<ResponsePaging<T>> GetPagedAsync<T>(
            this IQueryable<T> query,
            RequestPaging paging)
        {
            var response =
                await ResponsePaging<T>.CreateAsync(query, paging);

            return response;
        }

        public static async Task<ResponsePaging<T>> GetPagedAsync<T>(
            this IQueryable<T> query,
            int pageNumber,
            int pageSize,
            List<ExpressionParameter> filterParameter = null,
            string orderBy = null,
            OrderByType? orderByType = null)
        {
            var response =
                await ResponsePaging<T>.CreateAsync(query, pageNumber, pageSize, filterParameter, orderBy, orderByType);

            return response;
        }
    }
}