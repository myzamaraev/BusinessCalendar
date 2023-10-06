namespace BusinessCalendar.WebAPI.Models;

/// <summary>
/// model to provide information about user for SPA
/// </summary>
public class UserInfo
{
    public string? UserName { get; set; } 
    
    public List<string>? Roles { get; set; }
}