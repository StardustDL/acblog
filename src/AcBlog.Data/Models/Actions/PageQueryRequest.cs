namespace AcBlog.Data.Models.Actions
{
    public class PageQueryRequest : QueryRequest
    {
        public string Id { get; set; } = string.Empty;

        public string Layout { get; set; } = string.Empty;

        public string Route { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
    }
}
