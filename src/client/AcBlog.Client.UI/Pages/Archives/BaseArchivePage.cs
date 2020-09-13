using AcBlog.Client.UI.Shared;

namespace AcBlog.Client.UI.Pages.Archives
{
    public class BaseArchivePage : BasePage
    {
        protected override string Title
        {
            get => base.Title; set
            {
                value = string.IsNullOrEmpty(value) ? $"Archives" : $"{value} - Archives";
                base.Title = value;
            }
        }
    }
}
