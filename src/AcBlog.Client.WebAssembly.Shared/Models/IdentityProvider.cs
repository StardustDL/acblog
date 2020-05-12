namespace AcBlog.Client.WebAssembly.Models
{
    public class IdentityProvider
    {
        public bool Enable { get; set; } = false;

        public string Endpoint { get; set; } = "_configuration/AcBlog.Client.WebAssembly";

        public string RemoteRegisterPath { get; set; } = "Identity/Account/Register";

        public string RemoteProfilePath { get; set; } = "Identity/Account/Manage";
    }
}
