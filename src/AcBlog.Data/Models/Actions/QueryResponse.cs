namespace AcBlog.Data.Models.Actions
{
    public record QueryResponse<T>
    {
        public QueryStatus Status { get; init; }

        public T? Result { get; init; }
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
