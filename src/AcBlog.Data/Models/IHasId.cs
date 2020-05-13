namespace AcBlog.Data.Models
{
    public interface IHasId<T>
    {
        T Id { get; set; }
    }
}
