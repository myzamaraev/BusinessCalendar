namespace BusinessCalendar.WebAPI.Models;

/// <summary>
/// model to provide information about auth necessity and userInfo for SPA
/// </summary>
public class AuthInfo
{
    public bool IsAuthEnabled { get; set; }
    public UserInfo? UserInfo { get; set; }
}