namespace AcBlog.Data.Repositories.SqlServer.Models
{
    public interface IHasId<T>
    {
        T Id { get; set; }
    }
}
