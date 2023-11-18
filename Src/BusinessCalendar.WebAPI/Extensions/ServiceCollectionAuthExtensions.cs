using BusinessCalendar.WebAPI.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using OpenIdConnectOptions = BusinessCalendar.WebAPI.Options.OpenIdConnectOptions;

namespace BusinessCalendar.WebAPI.Extensions;

public static class ServiceCollectionAuthExtensions
{
    /// <summary>
    /// Adds authorization through oidc provider
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="showPII">show meaningful personal information (claims) for identity debug purposes</param>
    /// <returns></returns>
    /// <exception cref="Exception">Throws exception if no configuration section found for OpenIdConnectOptions</exception>
    public static IServiceCollection AddOpenIdConnectAuth(this IServiceCollection services, IConfiguration configuration, bool showPII = false)
    {
        var authOptionsSection = configuration.GetSection(AuthOptions.Section);
        services.Configure<AuthOptions>(authOptionsSection);
        
        var authSettings = authOptionsSection.Get<AuthOptions>();
        if (authSettings is not { UseOpenIdConnectAuth: true } )
        {
            return services;
        }
        
        var openIdConnectSettings = configuration.GetSection(OpenIdConnectOptions.Section).Get<OpenIdConnectOptions>();
        if (openIdConnectSettings == null)
        {
            throw new Exception($"Can't bind {OpenIdConnectOptions.Section} section from configuration to type {typeof(OpenIdConnectOptions)}");
        }

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //we use cookie scheme for challenge to manually prevent any redirects later.
                //oidc challenge is available with and only with calling /Account/Login endpoint from browser
                //All this tricks to return status 401/403 from BFF while using classic Authorization middleware 
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme; 
                options.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = "web";
                //options.ExpireTimeSpan = TimeSpan.FromSeconds(5);
                options.ExpireTimeSpan = TimeSpan.FromMinutes(authSettings.SessionCookieLifetimeMinutes);

                options.Events.OnRedirectToLogin  = context => 
                 {

                     if (context.Request.Path.StartsWithSegments("/api")
                         ||context.Request.Path.StartsWithSegments("/bff"))
                     {
                         //todo: replace with /bff uri for clearness?
                         //do not redirect/challenge another scheme for API calls (it's impossible with ajax calls).
                         context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                     }
                     return Task.CompletedTask;
                 };
                options.Events.OnRedirectToAccessDenied  = context => 
                {
                    if (context.Request.Path.StartsWithSegments("/api")
                        || context.Request.Path.StartsWithSegments("/bff"))
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    }
                    return Task.CompletedTask;
                };
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = openIdConnectSettings.Authority;
                options.ClientId = openIdConnectSettings.ClientId;
                options.ClientSecret = openIdConnectSettings.ClientSecret;

                options.ResponseType = OpenIdConnectResponseType.Code;
                options.ResponseMode = OpenIdConnectResponseMode.Query;

                options.Scope.Clear();
                options.Scope.Add("openid"); //Mandatory for OpenId connect
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.Scope.Add("offline_access");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
                
                //map claims from UserInfo, manual Mapping required here 
                options.GetClaimsFromUserInfoEndpoint = true;
                options.ClaimActions.MapJsonKey("role", "role"); 
                
                options.SaveTokens = true;
                options.MapInboundClaims = false;
                options.RequireHttpsMetadata = false;
            });

        services.AddAuthorization();
        
        if (showPII)
        {
            IdentityModelEventSource.ShowPII = true;
        }

        return services;
    }
}