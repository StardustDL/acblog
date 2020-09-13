using AcBlog.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

#pragma warning disable IDE1006 // 命名样式

namespace AcBlog.Tools.Sdk.Models.Text
{
    public class PageMetadata : MetadataBase<Page>
    {
        public string id { get; set; } = string.Empty;

        public string layout { get; set; } = string.Empty;

        public string route { get; set; } = string.Empty;

        public string title { get; set; } = string.Empty;

        public string[] features { get; set; } = Array.Empty<string>();

        public Dictionary<string, dynamic> properties { get; set; } = new Dictionary<string, dynamic>();

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
            features = data.Features.Items.ToArray();
            properties = new Dictionary<string, dynamic>(
                data.Properties.Raw.Select(
                    x => new KeyValuePair<string, dynamic>(
                        x.Key,
                        JsonConvert.DeserializeObject<dynamic>(x.Value))));
            route = data.Route;
        }

        public override void ApplyTo(Page data)
        {
            data.Id = id;
            data.Title = title;
            data.Features = new Feature(features);
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
            data.Properties = new PropertyCollection(new Dictionary<string, string>(
                properties.Select(
                    x => new KeyValuePair<string, string>(
                        x.Key,
                        JsonConvert.SerializeObject(x.Value)))));
        }
    }
}
