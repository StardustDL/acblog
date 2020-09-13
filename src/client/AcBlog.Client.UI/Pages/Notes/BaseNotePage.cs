using AcBlog.Client.UI.Shared;

namespace AcBlog.Client.UI.Pages.Notes
{
    public class BaseNotePage : BasePage
    {
        protected override string Title
        {
            get => base.Title; set
            {
                if (string.IsNullOrEmpty(value))
                    value = $"Notes";
                else
                    value = $"{value} - Notes";
                base.Title = value;
            }
        }
    }
}
