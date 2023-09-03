using System;
using BusinessCalendar.Domain.Enums;

namespace BusinessCalendar.Domain.Exceptions
{
    [Serializable]
    public class DocumentNotFoundClientException : ClientException
    {
        public DocumentNotFoundClientException(string message) : base(message, ErrorCode.NotFound)
        {
        }
    }
}