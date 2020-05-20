using AcBlog.Data.Models;

namespace AcBlog.Tools.SDK.Models.Text
{
    public class KeywordTextual : TextualBase<Keyword, KeywordTextual.Metadata>
    {
        public class Metadata
        {
            public string id { get; set; } = string.Empty;

            public string name { get; set; } = string.Empty;
        }

        protected override Metadata? GetMetadata(Keyword data) => new Metadata
        {
            id = data.Id,
            name = data.Name
        };

        protected override void SetMetadata(Keyword data, Metadata meta)
        {
            data.Id = meta.id;
            data.Name = meta.name;
        }
    }
}
