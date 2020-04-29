namespace AcBlog.Data.Providers
{
    public interface IProvider
    {

        ProviderContext? Context { get; set; }
    }
}
