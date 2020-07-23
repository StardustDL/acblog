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
        public static async Task Visited(this IStatisticService service, Post data)
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
                    Uri = data.GetStatisticUri(),
                    Payload = ""
                });
            }
            catch
            {

            }
        }

        public static async Task<int?> Count(this IStatisticService service, Post data)
        {
            if (service is null)
                return null;
            try
            {
                return (await service.Query(new Data.Models.Actions.StatisticQueryRequest
                {
                    Category = "Post",
                    Uri = data.GetStatisticUri(),
                    Pagination = new Data.Models.Actions.Pagination
                    {
                        PageSize = 1
                    }
                })).CurrentPage.TotalCount;
            }
            catch
            {
                return null;
            }
        }
    }
}
