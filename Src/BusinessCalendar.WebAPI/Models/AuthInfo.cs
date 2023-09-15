namespace BusinessCalendar.WebAPI.Models;

public class AuthInfo
{
    public bool IsAuthEnabled { get; set; }
    public UserInfo? UserInfo { get; set; }
}