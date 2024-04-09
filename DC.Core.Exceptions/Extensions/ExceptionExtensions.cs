using System.Net;
using Microsoft.Extensions.Logging;

namespace DC.Core.Exceptions.Extensions
{
	public static class ExceptionExtensions
    {
        public static HttpStatusCode GetHttpStatusCode(this Exception exception)
        {
            return exception.Data["StatusCode"] != null ?
                   (HttpStatusCode)exception.Data["StatusCode"] :
                   HttpStatusCode.InternalServerError;
        }

        public static bool HasLoggable(this Exception exception)
        {
            return exception.Data["HasLoggable"] != null ? (bool)exception.Data["HasLoggable"] : true;
        }

        public static LogLevel GetLogLevel(this Exception exception)
        {
            return exception.Data["LogLevel"] != null ? (LogLevel)exception.Data["LogLevel"] : LogLevel.Error;
        }

        public static string GetMessage(this Exception exception)
        {
            return exception.InnerException?.Message != null ? exception.InnerException.Message : exception.Message;
        }

        public static bool HasCustomException(this Exception exception)
        {
            return exception.Data["HasCustomException"] != null ? (bool)exception.Data["HasCustomException"] : false;
        }

        public static string GetUIMessage(this Exception exception, string? defaultMessage = null)
        {
            return exception.HasCustomException() ? (exception.Message ?? $"{defaultMessage}") : $"{defaultMessage}";
        }
    }
}
