namespace AcBlog.Client.WebAssembly.Models
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

    public class CommentServerSettings
    {
        public bool Enable { get; set; }

        public string Uri { get; set; }
    }

    public class StatisticServerSettings
    {
        public bool Enable { get; set; }

        public string Uri { get; set; }
    }
}
