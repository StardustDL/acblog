using AcBlog.Client.UI.Shared;

namespace AcBlog.Client.UI.Pages.Comments
{
    public class BaseCommentPage : BasePage
    {
        protected override string Title
        {
            get => base.Title; set
            {
                if (string.IsNullOrEmpty(value))
                    value = $"Comments";
                else
                    value = $"{value} - Comments";
                base.Title = value;
            }
        }
    }
}
