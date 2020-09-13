using AcBlog.Client.UI.Shared;

namespace AcBlog.Client.UI.Pages.Pages
{
    public class BasePagePage : BasePage
    {
        protected override string Title
        {
            get => base.Title; set
            {
                value = string.IsNullOrEmpty(value) ? $"Pages" : $"{value} - Pages";
                base.Title = value;
            }
        }
    }
}
