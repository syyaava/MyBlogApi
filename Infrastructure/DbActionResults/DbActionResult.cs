namespace Infrastructure.DbActionResults
{
    public class DbActionResult<T>
    {
        public T? Result { get; }
        public StatusCode Status { get; }  
        public Exception? Exception { get; }

        public DbActionResult(T? result) : this(result, StatusCode.Success) { }

        public DbActionResult(T? result, DbActionResult<T>.StatusCode status) : this(result, status, null) { }

        public DbActionResult(T? result, DbActionResult<T>.StatusCode status, Exception? exception)
        {
            Result = result;
            Status = status;
            Exception = exception;
        }

        public enum StatusCode
        {
            Success,
            NotFound,
            Error
        }
    }
}
