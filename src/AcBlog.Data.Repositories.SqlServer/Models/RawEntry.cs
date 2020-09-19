using AcBlog.Data.Models;

namespace AcBlog.Data.Repositories.SqlServer.Models
{
    public class RawEntry : IHasId<string>
    {
        public string Id { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;
    }
}
