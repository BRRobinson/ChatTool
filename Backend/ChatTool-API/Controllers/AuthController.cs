using ChatTool.API.Interfaces;
using ChatTool.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatTool.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthManager _authManager;

    public AuthController(ILogger<AuthController> logger, IAuthManager authManager)
    {
        _logger = logger;
        _authManager = authManager;
    }

    [HttpPost("Login")]
    public IActionResult Login(LoginRequest loginRequest)
    {
        try
        {
            var loginResult = _authManager.Login(loginRequest);
            if (loginResult.IsSuccess)
                return Ok(loginResult);

            return Unauthorized(loginResult);
        }
        catch (Exception e)
        {
            return StatusCode(500, ReturnResult<string>.Failed());
        }
    }

    [HttpPost("Register")]
    public IActionResult Register(LoginRequest loginRequest)
    {
        try
        {
            var registerResult = _authManager.Register(loginRequest);
            if (registerResult.IsSuccess)
                return Ok(registerResult);

            return Unauthorized(registerResult);
        }
        catch (Exception e)
        {
            return StatusCode(500, ReturnResult<string>.Failed());
        }
    }
}
