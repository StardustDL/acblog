namespace AcBlog.Tools.Sdk.Models
{
    public enum RemoteType
    {
        LocalFS,
        RemoteFS,
        Api
    }

    public class RemoteOption
    {
        public string Name { get; set; } = string.Empty;

        public string Uri { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public RemoteType Type { get; set; }
    }
}
