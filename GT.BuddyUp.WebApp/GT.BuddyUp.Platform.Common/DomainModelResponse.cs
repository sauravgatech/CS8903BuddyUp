using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using FluentValidation.Results;
using System.Net;

namespace GT.BuddyUp.Platform.Common
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable")]
    public class DomainModelResponse : Exception
    {
        private List<ResponseMessage> _responseMessages;
        private bool _hasError;
        private string finalMessage;

        public DomainModelResponse()
        {
            _responseMessages = new List<ResponseMessage>();
            _hasError = false;
        }

        public DomainModelResponse(ResponseCategory responseCategory, string propertyName, string message, string stackTrace = "", object onObject = null)
            : this()
        {
            if (responseCategory == ResponseCategory.Error)
                _hasError = true;
            _responseMessages.Add(new ResponseMessage(responseCategory, propertyName, message, stackTrace, onObject));
        }

        //Add Methods
        //public void addResponse(ResponseCategory responseCategory, string propertyName, string message, string stackTrace = "", object onObject = null)
        //{
        //    if (responseCategory == ResponseCategory.Error)
        //    {
        //        _hasError = true;
        //    }
        //    _responseMessages.Add(new ResponseMessage(responseCategory, propertyName, message, stackTrace, onObject));
        //}

        //public void addResponse(MessageCodes messageCode, string customMessage = null, object onObject = null, string stackTrace = null)
        //{
        //    string msg = string.Empty;
        //    if (string.IsNullOrWhiteSpace(customMessage))
        //        msg = messageCode.GetDescription();
        //    else
        //        msg = string.Format(messageCode.GetDescription() + " => " + customMessage);
        //    if ((int)messageCode >= 400 && (int)messageCode < 500)
        //    {
        //        _hasError = true;
        //        _responseMessages.Add(new ResponseMessage(ResponseCategory.Error, msg, onObject, stackTrace));
        //    }
        //    else if((int)messageCode >= 300 && (int)messageCode < 400)
        //    {
        //        _responseMessages.Add(new ResponseMessage(ResponseCategory.Warning, msg, onObject, stackTrace));
        //    }
        //    else if((int)messageCode >= 200 && (int)messageCode < 300)
        //    {
        //        _responseMessages.Add(new ResponseMessage(ResponseCategory.Information, msg, onObject, stackTrace));
        //    }

        //}

        public void addResponse(string propertyName, MessageCodes messageCode, string customMessage = null, object onObject = null, string stackTrace = null, HttpStatusCode? suggestedStatusCode = null, bool showCustomMessageOnly = false)
        {
            string msg = string.Empty;

            if (string.IsNullOrWhiteSpace(customMessage))
                msg = messageCode.GetDescription();

            if (showCustomMessageOnly && !string.IsNullOrWhiteSpace(customMessage))
                msg = customMessage;
            else
                msg = string.Format(messageCode.GetDescription() + " => " + customMessage);

            if ((int)messageCode >= 400 && (int)messageCode < 500)
            {
                _hasError = true;
                _responseMessages.Add(new ResponseMessage(ResponseCategory.Error, propertyName, msg, stackTrace, onObject, suggestedStatusCode));
            }
            else if ((int)messageCode >= 300 && (int)messageCode < 400)
            {
                _responseMessages.Add(new ResponseMessage(ResponseCategory.Warning, propertyName, msg, stackTrace, onObject, suggestedStatusCode));
            }
            else if ((int)messageCode >= 200 && (int)messageCode < 300)
            {
                _responseMessages.Add(new ResponseMessage(ResponseCategory.Information, propertyName, msg, stackTrace, onObject, suggestedStatusCode));
            }
        }

        public void addError(string propertyName, string message, string stackTrace = "", object onObject = null, HttpStatusCode? suggestedStatusCode = null)
        {
            _hasError = true;
            _responseMessages.Add(new ResponseMessage(ResponseCategory.Error, propertyName, message, stackTrace, onObject, suggestedStatusCode));
        }

        public void addAllResponses(DomainModelResponse domainModelResponse)
        {
            foreach (var resp in domainModelResponse._responseMessages)
            {
                this._responseMessages.Add(resp);
                if (resp.IsError)
                    _hasError = true;
            }
        }
        //public void addErrors(IList<ValidationFailure> failures)
        //{
        //    _hasError = true;
        //    foreach (var failure in failures)
        //    {
        //        _responseMessages.Add(new ResponseMessage(ResponseCategory.Error, failure.PropertyName, failure.ErrorMessage));
        //    }
        //}

        public void addWarning(string propertyName, string message, object onObject = null)
        {
            _responseMessages.Add(new ResponseMessage(ResponseCategory.Warning, propertyName, message, onObject));
        }

        public void addInformation(string propertyName, string message, object onObject = null)
        {
            _responseMessages.Add(new ResponseMessage(ResponseCategory.Information, propertyName, message, onObject));
        }

        public void addSuccessMessage(string customMessage = "")
        {
            string msg = string.Empty;
            if (string.IsNullOrWhiteSpace(customMessage))
                msg = MessageCodes.InfoSavedSuccessfully.GetDescription();
            else
                msg = string.Format(MessageCodes.InfoSavedSuccessfully.GetDescription() + " : " + customMessage);
            _responseMessages.Add(new ResponseMessage(ResponseCategory.Information, MessageCodes.InfoSavedSuccessfully.GetDescription(), null, ""));
        }

        //Get Methods

        public List<ResponseMessage> ResponseMessages
        {
            get
            {
                return _responseMessages;
            }
        }

        public bool HasErrorMessages
        {
            get
            {
                return _hasError;
            }
        }

        public bool IsSuccessful
        {
            get
            {
                return !_hasError;
            }
        }

        public List<ResponseMessage> ErrorMessages
        {
            get
            {
                List<ResponseMessage> responseMessages = new List<ResponseMessage>();
                foreach (var responseMessage in _responseMessages)
                {
                    if (responseMessage.ResponseCategory == ResponseCategory.Error)
                        responseMessages.Add(responseMessage);
                }
                if (responseMessages.Count > 0)
                    return responseMessages;
                return null;
            }
        }

        public List<ResponseMessage> AllMessages
        {
            get
            {
                return _responseMessages;
            }
        }


        public List<ResponseMessage> InfoMessages
        {
            get
            {
                List<ResponseMessage> responseMessages = new List<ResponseMessage>();
                foreach (var responseMessage in _responseMessages)
                {
                    if (responseMessage.ResponseCategory == ResponseCategory.Information)
                        responseMessages.Add(responseMessage);
                }
                if (responseMessages.Count > 0)
                    return responseMessages;
                return null;
            }
        }

        public List<ResponseMessage> WarningMessages
        {
            get
            {
                List<ResponseMessage> responseMessages = new List<ResponseMessage>();
                foreach (var responseMessage in _responseMessages)
                {
                    if (responseMessage.ResponseCategory == ResponseCategory.Warning)
                        responseMessages.Add(responseMessage);
                }
                if (responseMessages.Count > 0)
                    return responseMessages;
                return null;
            }
        }


        public string FinalMessage
        {
            get
            {
                var lastMsg = _responseMessages.LastOrDefault();
                if (lastMsg != null)
                {
                    //string[] finalMsg = lastMsg.Message.Split('>');
                    string msg = "";
                    //if (finalMsg.Count() > 0)
                    //    msg += finalMsg[1].Trim();
                    //else
                    msg += lastMsg.Message;
                    return msg;
                }
                return null;
            }
        }

        public HttpStatusCode? FinalHttpStatusCode
        {
            get
            {
                var lastMsg = _responseMessages.LastOrDefault();
                if (lastMsg != null)
                {
                    return lastMsg.SuggestedCode;
                }
                else
                    return null;
            }
        }

        //Chronological Order


        public string FormattedErrorMessage
        {
            get
            {
                string message = "";
                int counter = 1;
                foreach (var msg in ResponseMessages)
                {
                    message += "#" + counter.ToString() + " (" + msg.ResponseCategory.ToString() + ") " + msg.Message + ". ";
                    counter++;
                }
                return message;
            }
        }

        //Chronological Order
        public string FormattedSuccessMessage
        {
            get
            {
                string message = "";
                int counter = 1;
                foreach (var msg in ResponseMessages)
                {
                    message += "#" + counter.ToString() + " (" + msg.ResponseCategory.ToString() + ") " + msg.Message + ". ";
                    counter++;
                }
                return message;
            }
        }

    }
}
