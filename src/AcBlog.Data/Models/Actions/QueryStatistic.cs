using System;

namespace AcBlog.Data.Models.Actions
{
    public class QueryStatistic
    {
        static readonly Lazy<QueryStatistic> _empty = new Lazy<QueryStatistic>(new QueryStatistic { Count = 0 });

        public static QueryStatistic Empty() => _empty.Value;

        public int Count { get; set; }
    }
}
