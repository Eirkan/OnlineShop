namespace Product.Core.Domain.Request
{

    [Serializable]
    public class RequestHeader
    {
        public string Token { get; set; }

        public string UserSubjectId { get; set; }

        public string TrackId { get; set; }

        public DateTime RequestDate { get; set; }

        public RequestChannelType RequestChannelType { get; set; }

        public int? LanguageId { get; set; }

        public string ClientIp { get; set; }

        public static RequestHeader Create()
        {
            return new RequestHeader()
            {
                TrackId = string.Empty,
                RequestDate = DateTime.Now,
            };
        }
    }
}
