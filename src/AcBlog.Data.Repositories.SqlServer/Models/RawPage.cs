using AcBlog.Data.Models;
using System;
using System.Text.Json;

namespace AcBlog.Data.Repositories.SqlServer.Models
{
    public class RawPage : IHasId<string>
    {
        public string Id { get; set; } = string.Empty;

        public string Layout { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Route { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Features { get; set; } = string.Empty;

        public string Properties { get; set; } = string.Empty;

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public static RawPage From(Page value)
        {
            return new RawPage
            {
                Id = value.Id,
                Layout = value.Layout,
                Features = value.Features.ToString(),
                CreationTime = value.CreationTime,
                ModificationTime = value.ModificationTime,
                Title = value.Title,
                Content = value.Content,
                Properties = JsonSerializer.Serialize(value.Properties),
                Route = value.Route,
            };
        }

        public static Page To(RawPage value)
        {
            return new Page
            {
                Id = value.Id,
                Layout = value.Layout,
                Features = AcBlog.Data.Models.Feature.Parse(value.Features),
                CreationTime = value.CreationTime,
                ModificationTime = value.ModificationTime,
                Title = value.Title,
                Content = value.Content,
                Properties = JsonSerializer.Deserialize<PropertyCollection>(value.Properties),
                Route = value.Route,
            };
        }
    }
}
