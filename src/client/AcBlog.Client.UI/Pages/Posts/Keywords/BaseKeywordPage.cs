using AcBlog.Client.UI.Shared;

namespace AcBlog.Client.UI.Pages.Posts.Keywords
{
    public class BaseKeywordPage : BasePage
    {
        protected override string Title
        {
            get => base.Title; set
            {
                if (string.IsNullOrEmpty(value))
                    value = $"Keywords";
                else
                    value = $"{value} - Keywords";
                base.Title = value;
            }
        }
    }
}
