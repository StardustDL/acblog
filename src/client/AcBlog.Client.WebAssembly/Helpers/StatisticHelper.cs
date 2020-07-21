using AcBlog.Data.Models;
using AcBlog.Sdk;
using AcBlog.Sdk.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcBlog.Client.WebAssembly.Helpers
{
    static class StatisticHelper
    {
        public static async Task StatisticVisited(this IBlogService blogService, Post data)
        {
            try
            {
                var service = blogService.StatisticService;
                await service.Create(new Statistic
                {
                    Category = "Post",
                    CreationTime = DateTimeOffset.Now,
                    ModificationTime = DateTimeOffset.Now,
                    Uri = data.GetStatisticUri(),
                    Payload = ""
                });
            }
            catch
            {

            }
        }

        public static async Task<long?> StatisticCount(this IBlogService blogService, Post data)
        {
            try
            {
                var service = blogService.StatisticService;
                return await service.Count(new Data.Models.Actions.StatisticQueryRequest
                {
                    Category = "Post",
                    Uri = data.GetStatisticUri()
                });
            }
            catch
            {
                return null;
            }
        }
    }
}
