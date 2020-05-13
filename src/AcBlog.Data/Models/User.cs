using System.Collections.Generic;
using System.Text;

namespace AcBlog.Data.Models
{
    public class User : IHasId<string>
    {
        public string Id { get; set; } = string.Empty;

        public string Nickname { get; set; } = string.Empty;
    }
}
