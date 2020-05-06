using System;

namespace AcBlog.Client.WASM.Models
{
    public class BuildStatus
    {
        public string Commit { get; set; } = "None";

        public string Branch { get; set; } = "None";

        public string BuildDate { get; set; } = "None";

        public string Repository { get; set; } = "acblog/acblog";
    }
}
