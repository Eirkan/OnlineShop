using ErrorOr;

namespace Customer.Core.Domain.Response
{
    [Serializable]
    public class ResponseBase
    {
        public ResponseHeader Header { get; set; } = ResponseHeader.Create();

        public ResponsePaging Paging { get; set; } = ResponsePaging.Create();

        public static ResponseBase Create() => new ResponseBase() { };

        public static ResponseBase Create(string responseCode, string message)
        {
            var response = new ResponseBase();
            response.Header.SetMessage(responseCode, message);

            return response;
        }

        public static ResponseBase Create(params Error[] exceptions)
        {
            var response = new ResponseBase();
            response.Header.Errors = exceptions.ToList();

            return response;
        }

        public void SetMessage(string responseCode, string message)
        {
            Header.SetMessage(responseCode, message);
        }
    }

    [Serializable]
    public class ResponseBase<TResult> : ResponseBase
    {
        public TResult Result { get; set; }

        public ResponseBase()
        {
            Result = default(TResult);
        }

        public static ResponseBase<TResult> Create(TResult result)
        {
            return
                new ResponseBase<TResult>()
                {
                    Result = result,
                };
        }

        public static ResponseBase<TResult> Create(TResult result, ResponsePaging<TResult> paging)
        {
            return
                new ResponseBase<TResult>()
                {
                    Result = result,
                    Paging = ResponsePaging.Create(paging.TotalCount, paging.CurrentPage, paging.PageSize),
                };
        }

        public static new ResponseBase<TResult> Create(string responseCode, string message)
        {
            var response = new ResponseBase<TResult>();
            response.Header.SetMessage(responseCode, message);

            return response;
        }

        public static new ResponseBase<TResult> Create(params Error[] exceptions)
        {
            var response = new ResponseBase<TResult>();
            response.Header.Errors = exceptions.ToList();

            return response;
        }

        public static TResponse Create<TResponse>(TResult result)
            where TResponse : ResponseBase<TResult>, new()
        {
            var instance = new TResponse();
            instance.Result = (result != null) ? result : default(TResult);

            return instance;
        }

        public static TResponse Create<TResponse>(TResult result, string responseCode, string message)
            where TResponse : ResponseBase<TResult>, new()
        {
            var instance = Create<TResponse>(result);
            instance.SetMessage(responseCode, message);

            return instance;
        }
    }
}
