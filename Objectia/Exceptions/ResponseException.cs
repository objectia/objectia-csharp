using Objectia;

namespace Objectia.Exceptions
{
    public class ResponseException : APIException
    {
        public int Status { get; set; }
        public string Code { get; set; }

        public ResponseException(int status, string message, string code = null) : base(message)
        {
            this.Status = status;
            this.Code = code;
        }

        public ResponseException(Error error) : this(error.Status, error.Message, error.Code)
        {
        }
    }
}