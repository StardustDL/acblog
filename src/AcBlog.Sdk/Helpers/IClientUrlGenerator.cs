using AcBlog.Data.Models;

namespace AcBlog.Sdk.Helpers
{
    public interface IClientUrlGenerator
    {
        string Search(string query = "");
        string Archives();
        string Articles();
        string Categories();
        string Category(Category category);
        string Keyword(Keyword keyword);
        string Keywords();
        string Notes();
        string Page(Page page);
        string Post(Post post);
        string Posts();
        string Slides();
        string Comments();
    }
}