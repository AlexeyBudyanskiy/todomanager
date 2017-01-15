using System;
using System.Net;

namespace ToDoManager.WEB.Infrastructure.Exceptions
{
    public class TransientFaultException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public TransientFaultException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
