using AcBlog.Data.Models;
using System;

namespace AcBlog.Data.Repositories.SqlServer.Models
{
    public class RawStatistic : IHasId<string>
    {
        public string Id { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Uri { get; set; } = string.Empty;

        public string Payload { get; set; } = string.Empty;

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public static RawStatistic From(Statistic value)
        {
            return new RawStatistic
            {
                Id = value.Id,
                Category = value.Category,
                Payload = value.Payload,
                Uri = value.Uri,
                CreationTime = value.CreationTime,
                ModificationTime = value.ModificationTime,
            };
        }

        public static Statistic To(RawStatistic value)
        {
            return new Statistic
            {
                Id = value.Id,
                Category = value.Category,
                Payload = value.Payload,
                Uri = value.Uri,
                CreationTime = value.CreationTime,
                ModificationTime = value.ModificationTime,
            };
        }
    }
}
