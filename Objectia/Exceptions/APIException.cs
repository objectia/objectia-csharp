using System;

namespace Objectia.Exceptions
{
    public class APIException : Exception
    {
        public APIException() { }
        public APIException(string message) : base(message) { }
    }

}