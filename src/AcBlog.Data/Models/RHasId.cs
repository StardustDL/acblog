namespace AcBlog.Data.Models
{
    public record RHasId<T>
    {
        public T? Id { get; init; }
    }
}
