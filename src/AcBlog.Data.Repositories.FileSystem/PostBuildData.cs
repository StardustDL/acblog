using AcBlog.Data.Models;
using AcBlog.Data.Protections;
using System;

namespace AcBlog.Data.Repositories.FileSystem
{
    public class PostBuildData
    {
        public PostBuildData(Post raw)
        {
            Raw = raw;
        }

        public Post Raw { get; set; }

        public ProtectionKey? Key { get; set; }
    }
}
