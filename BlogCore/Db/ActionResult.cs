namespace BlogCore.Db
{
    public class ActionResult<T>
    {
        public T? Result { get; }
        public StatusCode Status { get; }
        public Exception? Exception { get; }

        public ActionResult(T? result) : this(result, StatusCode.Success) { }

        public ActionResult(T? result, StatusCode status) : this(result, status, null) { }

        public ActionResult(T? result, StatusCode status, Exception? exception)
        {
            Result = result;
            Status = status;
            Exception = exception;
        }
    }

    public enum StatusCode
    {
        Success,
        NotFound,
        Error
    }
}
