using System.ComponentModel;

namespace BusinessCalendar.Domain.Enums
{ 
    /// <summary>
    /// Error Codes
    /// </summary>
    public enum ErrorCode
    {
        [Description("Invalid Request Data")]
        InvalidRequestData = 10,
        [Description("Document Not Found")]
        NotFound = 20,
        [Description("Invalid Model State")]
        InvalidModelState = 30,
        [Description("Duplicate Key Violation")]
        DuplicateKey = 40,
    }
}