using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DC.Core.Exceptions
{
    public class BaseException : Exception
    {
        private const string _message = "Sunucu hatası";
        private readonly HttpStatusCode _statusCode = HttpStatusCode.InternalServerError;
        private readonly bool _hasLoggable = true;
        private readonly LogLevel _logLevel = LogLevel.Error;
        private readonly bool _hasCustomException = true;

        public BaseException(string? message = null, Exception? innerException = null) : base(message ?? _message, innerException)
        {
            base.Data.Add("StatusCode", _statusCode);
            base.Data.Add("HasLoggable", _hasLoggable);
            base.Data.Add("LogLevel", _logLevel);
            base.Data.Add("HasCustomException", _hasCustomException);
        }

        public BaseException(string? message = null) : this(message ?? _message, null)
        {
        }

        public BaseException(string message, HttpStatusCode statusCode) : this(message)
        {
            base.Data["StatusCode"] = statusCode;
        }
        public BaseException(string message, HttpStatusCode statusCode, bool hasLoggable) : this(message, statusCode)
        {
            base.Data["HasLoggable"] = hasLoggable;
        }

        public BaseException(string message, HttpStatusCode statusCode, bool hasLoggable, LogLevel logLevel) : this(message, statusCode, hasLoggable)
        {
            base.Data["LogLevel"] = logLevel;
        }

        public BaseException(string message, HttpStatusCode statusCode, LogLevel logLevel) : this(message, statusCode)
        {
            base.Data["LogLevel"] = logLevel;
        }
    }
}
