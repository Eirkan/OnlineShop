using Product.Core.Domain.ExpressionBuilder;
using System;
using System.Collections.Generic;

namespace Product.Core.Domain.Request
{
    [Serializable]
    public class RequestPaging
    {
        public int SkipCount { get; set; }

        public int TakeCount { get; set; }

        public OrderByType? OrderByType { get; set; }

        public string OrderColumn { get; set; }

        public List<ExpressionParameter> FilterParameter { get; set; } = new List<ExpressionParameter>();

        public static RequestPaging Create()
        {
            return
                new RequestPaging();
        }
    }
}
