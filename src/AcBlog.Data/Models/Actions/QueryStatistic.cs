using System;

namespace AcBlog.Data.Models.Actions
{
    public record QueryStatistic
    {
        static readonly Lazy<QueryStatistic> _empty = new Lazy<QueryStatistic>(new QueryStatistic { Count = 0 });

        public static QueryStatistic Empty() => _empty.Value;

        public int Count { get; init; }
    }
}
