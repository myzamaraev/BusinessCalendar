using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Exceptions;
using BusinessCalendar.WebAPI.Constants;
using BusinessCalendar.WebAPI.Models;
using BusinessCalendar.WebAPI.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BusinessCalendar.WebAPI.Controllers.BffV1;

public class AccountController : BffV1Controller
{
    private readonly IOptions<AuthOptions> _authOptions;

    public AccountController(IOptions<AuthOptions> authOptions)
    {
        _authOptions = authOptions;
    }
    
    [HttpGet]
    [Route("[action]")]
    [AllowAnonymous]
    public ActionResult<AuthInfo> UserInfo()
    {
        if (!_authOptions.Value.IsAuthEnabled)
        {
            return Ok(new AuthInfo
            {
                IsAuthEnabled = false,
            });
        }

        var authInfo = new AuthInfo { IsAuthEnabled = true };
        if (HttpContext.User.Identity is { IsAuthenticated: true })
        {
            authInfo.UserInfo = GetUserInfo();
        }

        return Ok(authInfo);
    }

    [HttpGet]
    [Route("[action]")]
    [AllowAnonymous]
    public IActionResult LogIn([FromQuery] string? redirectUri)
    {
        redirectUri = string.IsNullOrEmpty(redirectUri) ? "/" : redirectUri;

        if (HttpContext.User.Identity is not { IsAuthenticated: true })
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = redirectUri
            };

            return Challenge(authenticationProperties, OpenIdConnectDefaults.AuthenticationScheme);
        }

        return Redirect(redirectUri);
    }

    [HttpGet]
    [Route("[action]")]
    public IActionResult LogOut()
    {
        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = "/" //after signing out redirect to root SPA content
        };

        //sign out from an app without signing out from SSO session to not affect other apps
        return SignOut(authenticationProperties, CookieAuthenticationDefaults.AuthenticationScheme);
    }

    private UserInfo GetUserInfo()
    {
        var principal = HttpContext.User;
        var userName = principal.Identity?.Name ?? principal
            .FindFirst("email")?
            .Value;
        var roles = principal
            .FindAll("role")
            .Select(x => x.Value)
            .Distinct()
            .Where(role => BcRoles.RoleList.Any(bcRole => role == bcRole))
            .ToList();
        
        return new UserInfo
        {
            UserName = userName,
            Roles = roles
        };
    }
}