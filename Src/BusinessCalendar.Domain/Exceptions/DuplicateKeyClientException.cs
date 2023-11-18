using BusinessCalendar.Domain.Enums;

namespace BusinessCalendar.Domain.Exceptions;

[Serializable]
public class DuplicateKeyClientException : ClientException
{
    public DuplicateKeyClientException(string entityName, string id) 
        : base($"{entityName} already exists: {id}", ErrorCode.DuplicateKey)
    {
            
    }
}