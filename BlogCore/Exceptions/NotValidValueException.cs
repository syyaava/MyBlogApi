using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Exceptions
{
    public class NotValidValueException : Exception
    {
        public object? Object { get; init; } 

        public NotValidValueException() : base()
        {
            Object = null;
        }

        public NotValidValueException(string message) : base(message)
        {
            Object = null;
        }

        public NotValidValueException(object? obj) : base ($"Object: {obj.ToString()} not valid.")
        {
            Object = obj;
        }

        public NotValidValueException(string message, object? obj) : base(message)
        {
            Object = obj;
        }
    }
}
