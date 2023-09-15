namespace BusinessCalendar.WebAPI.Options;

public class AuthOptions
{
    public const string Section = "Auth";
    public bool  UseOpenIdConnectAuth { get; set; }
    
    public short SessionCookieLifetimeMinutes { get; set; }
    
    public bool IsAuthEnabled => UseOpenIdConnectAuth;
}