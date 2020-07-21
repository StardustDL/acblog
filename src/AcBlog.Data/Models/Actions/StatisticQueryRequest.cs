namespace AcBlog.Data.Models.Actions
{
    public class StatisticQueryRequest : QueryRequest
    {
        public string Category { get; set; } = string.Empty;

        public string Uri { get; set; } = string.Empty;

        public string Data { get; set; } = string.Empty;
    }
}
