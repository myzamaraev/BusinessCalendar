using System;
using BusinessCalendar.Domain.Enums;

namespace BusinessCalendar.Domain.Exceptions
{
    [Serializable]
    public class ArgumentClientException: ClientException
    {
        public ArgumentClientException(string propertyName, string value) 
            : base($"Argument {propertyName} can't have value {value}",  ErrorCode.InvalidRequestData)
        {
        }
        
        public ArgumentClientException(string propertyName) : base($"Argument {propertyName} can't be empty", ErrorCode.InvalidRequestData)
        {
        }
    }
}