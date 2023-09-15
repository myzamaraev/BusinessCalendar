namespace BusinessCalendar.WebAPI.Options;

/// <summary>
/// OpenId Connect client options
/// </summary>
public class OpenIdConnectOptions
{
    public const string Section = "OpenIdConnect";
    public string Authority { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}