using Objectia;

namespace Objectia.Exceptions
{
    public class ResponseException : APIException
    {
        public int Status { get; set; }

        public ResponseException(int status, string message) : base(message)
        {
            this.Status = status;
        }

        public ResponseException(Error error) : this(error.Status, error.Message)
        {
        }

    }
}