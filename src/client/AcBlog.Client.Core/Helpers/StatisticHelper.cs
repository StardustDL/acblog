using AcBlog.Data.Models;
using AcBlog.Sdk;
using AcBlog.Services;
using AcBlog.Services.Generators;
using System;
using System.Threading.Tasks;

namespace AcBlog.Client.Helpers
{
    public static class StatisticHelper
    {
        public static async Task Visited(this IStatisticService service, IClientUriGenerator urlGenerator, Post data)
        {
            if (service is null)
                return;
            try
            {
                await service.Create(new Statistic
                {
                    Category = "Post",
                    CreationTime = DateTimeOffset.Now,
                    ModificationTime = DateTimeOffset.Now,
                    Uri = urlGenerator.Post(data),
                    Payload = ""
                });
            }
            catch
            {

            }
        }

        public static async Task<int?> Count(this IStatisticService service, IClientUriGenerator urlGenerator, Post data)
        {
            if (service is null)
                return null;
            try
            {
                return (await service.Statistic(new Data.Models.Actions.StatisticQueryRequest
                {
                    Category = "Post",
                    Uri = urlGenerator.Post(data),
                    Pagination = new Data.Models.Actions.Pagination
                    {
                        PageSize = int.MaxValue
                    }
                })).Count;
            }
            catch
            {
                return null;
            }
        }
    }
}
