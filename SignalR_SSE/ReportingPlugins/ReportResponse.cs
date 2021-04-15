namespace SignalR_SSE.ReportingPlugins
{
    public class ReportResponse
    {
        public ReportResponse(object data, string contentType)
        {
            Data = data;
            ContentType = contentType;
        }

        public object Data { get; set; }
        public string ContentType { get; set; }
    }
}
