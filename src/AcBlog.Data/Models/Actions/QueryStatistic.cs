namespace AcBlog.Data.Models.Actions
{
    public class QueryStatistic
    {
        public static QueryStatistic Empty()
        {
            return new QueryStatistic { Count = 0 };
        }

        public int Count { get; set; }
    }
}
