namespace AcBlog.Data.Repositories
{
    public interface IRepository
    {

        RepositoryAccessContext Context { get; set; }
    }
}
