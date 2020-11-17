namespace AcBlog.Tools.Sdk.Models.Text
{
    public abstract class MetadataBase<T>
    {
        public abstract T ApplyTo(T data);
    }
}
