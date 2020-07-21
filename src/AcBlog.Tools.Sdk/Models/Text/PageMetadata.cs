using AcBlog.Data.Models;
using System;
using System.Collections.Generic;

namespace AcBlog.Tools.Sdk.Models.Text
{
    public class PageMetadata : MetadataBase<Page>
    {
        public string id { get; set; } = string.Empty;

        public string layout { get; set; } = string.Empty;

        public string route { get; set; } = string.Empty;

        public string title { get; set; } = string.Empty;

        public Dictionary<string, string> properties { get; set; } = new Dictionary<string, string>();

        public string creationTime { get; set; } = string.Empty;

        public string modificationTime { get; set; } = string.Empty;

        public PageMetadata() { }

        public PageMetadata(Page data)
        {
            id = data.Id;
            title = data.Title;
            layout = data.Layout;
            creationTime = data.CreationTime.ToString();
            modificationTime = data.ModificationTime.ToString();
            properties = data.Properties;
            route = data.Route;
        }

        public override void ApplyTo(Page data)
        {
            data.Id = id;
            data.Title = title;
            if (DateTimeOffset.TryParse(creationTime, out var _creationTime))
            {
                data.CreationTime = _creationTime;
            }
            if (DateTimeOffset.TryParse(modificationTime, out var _modificationTime))
            {
                data.ModificationTime = _modificationTime;
            }
            data.Layout = layout;
            data.Route = route;
            data.Properties = properties;
        }
    }
}
