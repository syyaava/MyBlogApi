namespace BlogCore.Exceptions
{
    public class NotFoundInDbException<T> : NotFoundException<T>
    {
        public NotFoundInDbException() :base() { }

        public NotFoundInDbException(string message) : base(message) { }

        public NotFoundInDbException(T obj) : base($"Object of type \"{obj?.GetType()}\" not found in Db. Object to string: {obj?.ToString()}.") { }

        public NotFoundInDbException(string message, T exceptionObject) : base(message, exceptionObject) { }
    }
}
