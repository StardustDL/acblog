﻿using AcBlog.SDK.Filters;

namespace AcBlog.SDK
{
    public static class PostServiceExtensions
    {
        public static PostArticleFilter CreateArticleFilter(this IPostService service)
        {
            return new PostArticleFilter(service);
        }

        public static PostSlidesFilter CreateSlidesFilter(this IPostService service)
        {
            return new PostSlidesFilter(service);
        }
    }
}