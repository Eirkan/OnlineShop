using System;

namespace Product.Core.Domain.Request
{
    [Serializable]
    public class RequestBase
    {
        public RequestHeader Header { get; set; } = RequestHeader.Create();
        public RequestPaging Paging { get; set; } = RequestPaging.Create();
    }
}
