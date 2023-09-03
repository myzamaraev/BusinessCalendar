using System;
using BusinessCalendar.Domain.Enums;

namespace BusinessCalendar.Domain.Exceptions
{
    [Serializable]
    public class DuplicateKeyClientException : ClientException
    {
        public DuplicateKeyClientException(string message) : base(message, ErrorCode.DuplicateKey)
        {
            
        }
    }
}