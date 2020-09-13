using AcBlog.Client.UI.Shared;

namespace AcBlog.Client.UI.Pages.Settings
{
    public class BaseSettingsPage : BasePage
    {
        protected override string Title
        {
            get => base.Title; set
            {
                if (string.IsNullOrEmpty(value))
                    value = $"Settings";
                else
                    value = $"{value} - Settings";
                base.Title = value;
            }
        }
    }
}
