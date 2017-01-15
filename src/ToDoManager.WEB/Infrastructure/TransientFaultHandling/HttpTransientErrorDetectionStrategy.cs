using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using ToDoManager.WEB.Infrastructure.Exceptions;

namespace ToDoManager.WEB.Infrastructure.TransientFaultHandling
{
    public class HttpTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        private readonly List<HttpStatusCode> _statusCodes =
            new List<HttpStatusCode>
            {
            HttpStatusCode.GatewayTimeout,
            HttpStatusCode.RequestTimeout,
            HttpStatusCode.ServiceUnavailable,
            HttpStatusCode.BadRequest
            };

        public HttpTransientErrorDetectionStrategy(bool isNotFoundAsTransient = false)
        {
            if (isNotFoundAsTransient)
            {
                _statusCodes.Add(HttpStatusCode.NotFound);
            }
        }

        public bool IsTransient(Exception ex)
        {
            var transientFaultException = ex as TransientFaultException;
            return transientFaultException != null 
                && _statusCodes.Contains(transientFaultException.StatusCode);
        }
    }
}
