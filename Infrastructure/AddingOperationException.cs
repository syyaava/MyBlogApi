namespace Infrastructure
{
    public class AddingOperationException : Exception
    {
        public object? ExceptionObject { get; }
        public AddingOperationException() { }
        public AddingOperationException(string message) : base(message) { }
        public AddingOperationException(string message, object? obj) : base(message)
        {
            ExceptionObject = obj;
        }
    }
}
