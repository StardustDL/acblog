using AcBlog.Data.Models;
using System;

namespace AcBlog.Data.Repositories.SqlServer.Models
{
    public class RawLayout : IHasId<string>
    {
        public string Id { get; set; } = string.Empty;

        public string Template { get; set; } = string.Empty;

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public static RawLayout From(Layout value)
        {
            return new RawLayout
            {
                Id = value.Id,
                Template = value.Template,
                CreationTime = value.CreationTime,
                ModificationTime = value.ModificationTime,
            };
        }

        public static Layout To(RawLayout value)
        {
            return new Layout
            {
                Id = value.Id,
                Template = value.Template,
                CreationTime = value.CreationTime,
                ModificationTime = value.ModificationTime,
            };
        }
    }
}
