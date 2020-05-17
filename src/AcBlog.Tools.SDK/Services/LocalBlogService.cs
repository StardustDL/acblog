using AcBlog.SDK;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcBlog.Tools.SDK.Services
{
    public class LocalBlogService : IBlogService
    {
        public IUserService UserService => throw new NotImplementedException();

        public IPostService PostService => throw new NotImplementedException();

        public IKeywordService KeywordService => throw new NotImplementedException();

        public ICategoryService CategoryService => throw new NotImplementedException();
    }
}
