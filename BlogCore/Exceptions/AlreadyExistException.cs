using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Exceptions
{
    public class AlreadyExistException : Exception
    {
        public object? Object { get; }
        public AlreadyExistException() : base() { }
        public AlreadyExistException(string message) : base(message) { }
        public AlreadyExistException(object? obj)
        {
            Object = obj;
        }
        public AlreadyExistException(string message, object obj) : base(message)
        {
            Object = obj;
        }
    }
}
