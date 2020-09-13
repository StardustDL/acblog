using AcBlog.Client.UI.Shared;

namespace AcBlog.Client.UI.Pages.Categories
{
    public class BaseCategoryPage : BasePage
    {
        protected override string Title
        {
            get => base.Title; set
            {
                if (string.IsNullOrEmpty(value))
                    value = $"Categories";
                else
                    value = $"{value} - Categories";
                base.Title = value;
            }
        }
    }
}
