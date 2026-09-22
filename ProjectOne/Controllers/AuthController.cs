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
    public async Task<ActionResult<string>> Register(
        [FromServices] RegisterHandler handler, [FromBody] RegisterRequest request)
    {
        var result = await handler.RegisterAsync(request);
        return result.IsSuccess ? Ok(new { token = result.Value }) : result.Error.ToResponse();
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(
        [FromServices] LoginHandler handler, [FromBody] LoginRequest request)
    {
        var result = await handler.LoginAsync(request);
        return result.IsSuccess ? Ok(new { token = result.Value }) : result.Error.ToResponse();
    }
}