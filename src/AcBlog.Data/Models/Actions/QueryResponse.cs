namespace AcBlog.Data.Models.Actions
{
    public class QueryResponse<T>
    {
        public QueryStatus Status { get; set; }

        public T? Result { get; set; }
    }

    public static class QueryResponse
    {
        public static QueryResponse<T> Error<T>()
        {
            return new QueryResponse<T>
            {
                Status = QueryStatus.Error
            };
        }

        public static QueryResponse<T> Success<T>(T value)
        {
            return new QueryResponse<T>
            {
                Status = QueryStatus.Success,
                Result = value,
            };
        }
    }
}
