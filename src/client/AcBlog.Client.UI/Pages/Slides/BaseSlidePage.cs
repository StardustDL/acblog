using AcBlog.Client.UI.Shared;

namespace AcBlog.Client.UI.Pages.Slides
{
    public class BaseSlidePage : BasePage
    {
        protected override string Title
        {
            get => base.Title; set
            {
                if (string.IsNullOrEmpty(value))
                    value = $"Slides";
                else
                    value = $"{value} - Slides";
                base.Title = value;
            }
        }
    }
}
