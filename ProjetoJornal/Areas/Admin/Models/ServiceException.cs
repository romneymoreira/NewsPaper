using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoJornal.Areas.Admin.Models
{
    public class ServiceException : Exception
    {
        public enum ServiceExceptionType
        {
            EntityNotFound = 1
        }

        public string ServiceName { get; private set; }
        public string MethodName { get; private set; }
        public string Message { get; private set; }
        public string FullMessage
        {
            get
            {
                if (!string.IsNullOrEmpty(MethodName))
                    return string.Format("'{0}/{1}': '{2}'", ServiceName, MethodName, Message);
                else if (!string.IsNullOrEmpty(ServiceName))
                    return string.Format("'{0}': '{1}'", ServiceName, Message);
                else
                    return Message;
            }
        }

        public ServiceException()
            : base() { }
        public ServiceException(string message, params object[] args)
            : base(string.Format(message, args))
        {
            this.Message = message;
        }
        public ServiceException(string serviceName, string message)
            : base(message)
        {
            this.ServiceName = serviceName;
            this.Message = message;
        }

        public ServiceException(string serviceName, string method, string message)
            : this(serviceName, message)
        {
            this.MethodName = method;
        }

        public ServiceException(string serviceName, string method, string message, params object[] args)
            : this(serviceName, method, string.Format(message, args)) { }
    }
}