namespace AcBlog.Client.Models
{
    public class ServerSettings
    {
        public MainServerSettings Main { get; set; } = new MainServerSettings();

        public CommentServerSettings Comment { get; set; } = new CommentServerSettings();

        public StatisticServerSettings Statistic { get; set; } = new StatisticServerSettings();

        public IdentityServerSettings Identity { get; set; } = new IdentityServerSettings();

        public string BaseAddress { get; set; }
    }

    public class IdentityServerSettings
    {
        public bool Enable { get; set; } = false;

        public string Endpoint { get; set; } = "_configuration/AcBlog.Client.WebAssembly";

        public string RemoteRegisterPath { get; set; } = "Identity/Account/Register";

        public string RemoteProfilePath { get; set; } = "Identity/Account/Manage";
    }

    public class MainServerSettings
    {
        public string Uri { get; set; }

        public bool IsStatic { get; set; }
    }

    public enum CommentServerType
    {
        Disable,
        Main,
        Loment
    }

    public class CommentServerSettings
    {
        public CommentServerType Type { get; set; }

        public string Uri { get; set; }
    }

    public enum StatisticServerType
    {
        Disable,
        Main,
        Listat
    }

    public class StatisticServerSettings
    {
        public StatisticServerType Type { get; set; }

        public string Uri { get; set; }
    }
}
