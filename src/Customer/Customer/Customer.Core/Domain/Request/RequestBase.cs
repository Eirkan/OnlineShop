using System;

namespace Customer.Core.Domain.Request
{
    [Serializable]
    public class RequestBase
    {
        public RequestHeader Header { get; set; } = RequestHeader.Create();
        public RequestPaging Paging { get; set; } = RequestPaging.Create();
    }
}
