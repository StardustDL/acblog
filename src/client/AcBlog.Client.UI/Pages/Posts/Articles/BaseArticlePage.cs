using AcBlog.Client.UI.Shared;

namespace AcBlog.Client.UI.Pages.Posts.Articles
{
    public class BaseArticlePage : BasePage
    {
        protected override string Title
        {
            get => base.Title; set
            {
                if (string.IsNullOrEmpty(value))
                    value = $"Articles";
                else
                    value = $"{value} - Articles";
                base.Title = value;
            }
        }
    }
}
