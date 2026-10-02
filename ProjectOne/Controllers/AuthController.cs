using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectOne.Application.Auth.Login;
using ProjectOne.Application.Auth.Register;
using ProjectOne.Extensions;

namespace ProjectOne.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult> Register(
        [FromServices] RegisterHandler handler, [FromBody] RegisterRequest request)
    {
        var result = await handler.RegisterAsync(request);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        SetTokenCookie(result.Value);
        return Ok();
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login(
        [FromServices] LoginHandler handler, [FromBody] LoginRequest request)
    {
        var result = await handler.LoginAsync(request);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        SetTokenCookie(result.Value);
        return Ok();
    }
    
    [HttpPost("logout")]
    [Authorize]
    public ActionResult Logout()
    {
        Response.Cookies.Delete("access_token");
        return Ok();
    }
    
    private void SetTokenCookie(string token)
    {
        Response.Cookies.Append("access_token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddMinutes(120)
        });
    }
}