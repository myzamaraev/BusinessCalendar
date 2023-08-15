using System;
using BusinessCalendar.Domain.Enums;

namespace BusinessCalendar.Domain.Exceptions
{
    [Serializable]
    public  class ClientException : Exception
    {
        public ErrorCode ErrorCode { get; set; }
        public object? Details { get; set; }

        public ClientException(string message, ErrorCode errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public ClientException(string message, ErrorCode errorCode, object details) : base(message)
        {
            ErrorCode = errorCode;
            Details = details;
        }
    }
}