using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Exceptions
{
    public class NotFoundException<T> : Exception
    {
        public T? ExceptionObject { get; init; }

        public NotFoundException() : base() { }

        public NotFoundException(string message) : base(message)
        {
            ExceptionObject = default(T);
        }

        public NotFoundException(T? exceptionObject) : base($"Object of type {typeof(T)} not found.")
        {
            ExceptionObject = exceptionObject;
        }

        public NotFoundException(string message, T exceptionObject) : base(message)
        {
            ExceptionObject = exceptionObject;
        }
    }
}
