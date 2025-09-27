using ChatTool.API.Interfaces;
using ChatTool.API.Models;
using ChatTool.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatTool.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUserManager _userManager;

    public UsersController(ILogger<UsersController> logger, IUserManager userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<ReturnResult<List<User>>>> GetUsers()
    {
        try
        {
            var usersResult = await _userManager.GetUsers();
            if (usersResult.IsSuccess)
                return Ok(usersResult);

            return BadRequest(usersResult);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, ReturnResult<List<User>>.Failed());
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReturnResult<User>>> GetUser(int id)
    {
        try
        {
            var usersResult = await _userManager.GetUserById(id);
            if (usersResult.IsSuccess)
                return Ok(usersResult);

            return BadRequest(usersResult);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, ReturnResult<User>.Failed());
        }
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<ReturnResult<User>>> GetUser(string username)
    {
        try
        {
            var usersResult = await _userManager.GetUserByUsername(username);
            if (usersResult.IsSuccess)
                return Ok(usersResult);

            return BadRequest(usersResult);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, ReturnResult<User>.Failed());
        }
    }
}
