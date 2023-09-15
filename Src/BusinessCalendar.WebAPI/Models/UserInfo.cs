namespace BusinessCalendar.WebAPI.Models;

public class UserInfo
{
    public string? UserName { get; set; } 
    
    public List<string>? Roles { get; set; }
}