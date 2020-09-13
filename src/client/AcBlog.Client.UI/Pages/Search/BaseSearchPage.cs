using AcBlog.Client.UI.Shared;

namespace AcBlog.Client.UI.Pages.Search
{
    public class BaseSearchPage : BasePage
    {
        protected override string Title
        {
            get => base.Title; set
            {
                value = string.IsNullOrEmpty(value) ? $"Search" : $"{value} - Search";
                base.Title = value;
            }
        }
    }
}
