using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace GT.BuddyUp.Platform.Common
{
    public enum ResponseCategory
    {
        Information,
        Warning,
        Error
    }
    public class ResponseMessage
    {
        private readonly ResponseCategory _responseCategory;
        private readonly string _message;
        private readonly string _propertyName;
        private readonly object _onObject;
        private readonly string _stackTrace;
        private HttpStatusCode _suggestedCode;

        public ResponseMessage(ResponseCategory responseCategory, string propertyName, string message)
        {
            _responseCategory = responseCategory;
            _propertyName = propertyName;
            _message = message;
            _onObject = null;
            _suggestedCode = HttpStatusCode.Unused;
        }

        public ResponseMessage(ResponseCategory responseCategory, string message, object onObject, string stackTrace)
        {
            _responseCategory = responseCategory;
            _propertyName = null;
            _message = message;
            _onObject = onObject;
            _stackTrace = stackTrace;
            _suggestedCode = HttpStatusCode.Unused;
        }

        public ResponseMessage(ResponseCategory responseCategory, string propertyName, string message, string stackTrace)
        {
            _responseCategory = responseCategory;
            _propertyName = propertyName;
            _message = message;
            _onObject = null;
            _stackTrace = stackTrace;
            _suggestedCode = HttpStatusCode.Unused;
        }


        public ResponseMessage(ResponseCategory responseCategory, string propertyName, string message, object onObject)
        {
            _responseCategory = responseCategory;
            _propertyName = propertyName;
            _message = message;
            _onObject = onObject;
            _suggestedCode = HttpStatusCode.Unused;
        }

        public ResponseMessage(ResponseCategory responseCategory, string propertyName, string message, string stackTrace, object onObject)
        {
            _responseCategory = responseCategory;
            _propertyName = propertyName;
            _message = message;
            _stackTrace = stackTrace;
            _onObject = onObject;
            _suggestedCode = HttpStatusCode.Unused;
        }

        public ResponseMessage(ResponseCategory responseCategory, string propertyName, string message, string stackTrace, object onObject, HttpStatusCode? suggestedCode)
        {
            _responseCategory = responseCategory;
            _propertyName = propertyName;
            _message = message;
            _stackTrace = stackTrace;
            _onObject = onObject;
            if (suggestedCode.HasValue)
                _suggestedCode = (HttpStatusCode)suggestedCode;
        }

        public string Message
        {
            get
            {
                return _message;
            }
        }

        public string StackTrace
        {
            get
            {
                return _stackTrace;
            }
        }

        public string PropertyName
        {
            get
            {
                return _propertyName;
            }
        }

        public ResponseCategory ResponseCategory
        {
            get
            {
                return _responseCategory;
            }
        }

        public HttpStatusCode SuggestedCode
        {
            get
            {
                return _suggestedCode;
            }
        }

        public bool IsError
        {
            get
            {
                if (this._responseCategory == Common.ResponseCategory.Error)
                    return true;
                return false;
            }
        }
    }
}

