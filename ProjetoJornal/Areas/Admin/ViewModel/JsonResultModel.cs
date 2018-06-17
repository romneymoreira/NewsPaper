using ProjetoJornal.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoJornal.Areas.Admin.ViewModel
{
    [Serializable]
    public class JsonResultModel
    {
        [Serializable]
        public class JsonResultError
        {
            public string ServiceName { get; set; }
            public string Message { get; set; }
            public JsonResultError(string serviceName, string message)
            {
                ServiceName = serviceName;
                Message = message;
            }
        }

        public object Data { get; set; }
        public string Message { get; private set; }
        public JsonResultError StackTrace { get; set; }

        public bool Success
        {
            get { return Message == "OK"; }
        }

        public JsonResultModel()
        {
            Message = "OK";
        }
        public JsonResultModel(object data)
            : this()
        {
            Data = data;
        }

        private JsonResultModel(string message)
        {
            Message = message;
        }

        private JsonResultModel(string message, JsonResultError stackTrace)
            : this(message: message)
        {
            StackTrace = stackTrace;
        }

        public static JsonResultModel CreateError(Exception ex)
        {
            if (ex.GetType() == typeof(ServiceException))
                return new JsonResultModel(
                    message: ex.Message,
                    stackTrace: new JsonResultError(((ServiceException)ex).ServiceName, ((ServiceException)ex).FullMessage));
            else
                return new JsonResultModel(message: ex.Message);
        }
    }
}