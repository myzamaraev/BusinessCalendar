using BusinessCalendar.WebAPI.Constants;
using BusinessCalendar.WebAPI.Extensions;
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
    private const string RootSpaContentUri = "/";

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
        redirectUri = string.IsNullOrEmpty(redirectUri) ? RootSpaContentUri : redirectUri;
        if (HttpContext.User.Identity is { IsAuthenticated: true })
        {
            return Redirect(redirectUri);
        }
        
        var authenticationProperties = new AuthenticationProperties { RedirectUri = redirectUri };
        return Challenge(authenticationProperties, OpenIdConnectDefaults.AuthenticationScheme);
    }

    [HttpGet]
    [Route("[action]")]
    public IActionResult LogOut()
    {
        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = RootSpaContentUri
        };

        //sign out from an app without signing out from SSO session to not affect other apps
        return SignOut(authenticationProperties, CookieAuthenticationDefaults.AuthenticationScheme);
    }

    [HttpGet]
    [Route("[action]")]
    public IActionResult SingleLogOut()
    {
        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = RootSpaContentUri
        };

        return SignOut(authenticationProperties, CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme);
    }

    private UserInfo GetUserInfo()
    {
        var principal = HttpContext.User;
        return new UserInfo
        {
            UserName = principal?.GetUserName() ?? principal?.GetEmail(),
            Roles = principal?.GetRoles()
        };
    }
}