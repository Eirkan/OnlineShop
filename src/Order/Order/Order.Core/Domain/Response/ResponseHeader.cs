using ErrorOr;

namespace Order.Core.Domain.Response
{
    [Serializable]
    public class ResponseHeader
    {
        public bool HasError
        {
            get
            {
                return Errors.Count == 0 ? false : true;
            }
        }
        public string Status
        {
            get
            {
                return HasError ? "success" : "fail";
            }
        }

        public List<Error> Errors { get; set; } = new List<Error>();

        public static ResponseHeader Create()
        {
            return new ResponseHeader();
        }

        public static ResponseHeader Create(Error exception)
        {
            var response = new ResponseHeader();
            response.Errors.Add(exception);
            return response;
        }

        public void SetMessage(string responseCode, string message)
        {
            Errors.Add(Error.Failure(responseCode, message));
        }

    }

    [Serializable]
    public class ResponseMessageItem
    {
        public string ResponseCode { get; set; }

        public string Message { get; set; }

        public static ResponseMessageItem Create(string responseCode, string message)
        {
            return new ResponseMessageItem { Message = message, ResponseCode = responseCode, };
        }
    }
}