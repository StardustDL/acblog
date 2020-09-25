using AcBlog.Data.Models;

namespace AcBlog.Services.Generators
{
    public interface IClientUriGenerator
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