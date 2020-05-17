namespace AcBlog.Tools.SDK.Models
{
    public class DbItem
    {
        public string Id { get; set; } = string.Empty;

        public string? OriginRemoteHash { get; set; }

        public string? OriginLocalHash { get; set; }
    }
}
