using AcBlog.Client.UI.Shared;

namespace AcBlog.Client.UI.Pages.Posts
{
    public class BasePostPage : BasePage
    {
        protected override string Title
        {
            get => base.Title; set
            {
                value = string.IsNullOrEmpty(value) ? $"Posts" : $"{value} - Posts";
                base.Title = value;
            }
        }
    }
}
