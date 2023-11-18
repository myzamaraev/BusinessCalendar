using BusinessCalendar.Domain.Enums;

namespace BusinessCalendar.Domain.Exceptions;

[Serializable]
public class DocumentNotFoundClientException : ClientException
{
    public DocumentNotFoundClientException(string entityName, string id)
        : base($"No {entityName} found with id: {id}", ErrorCode.NotFound)
    {
    }
}