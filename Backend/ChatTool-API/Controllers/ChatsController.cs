using ChatTool.API.Interfaces;
using ChatTool.API.Managers;
using ChatTool.API.Models;
using ChatTool.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatTool.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ChatsController : ControllerBase
{
    private readonly ILogger<ChatsController> _logger;
    private readonly IChatManager _chatManager;

    public ChatsController(ILogger<ChatsController> logger, IChatManager chatManager)
    {
        _logger = logger;
        _chatManager = chatManager;
    }

    [HttpGet]
    public async Task<ActionResult<ReturnResult<List<Chat>>>> GetChat()
    {
        try
        {
            var chatsResult = await _chatManager.GetUserChats(User.FindFirst("username")!.Value);
            if (chatsResult.IsSuccess)
                return Ok(chatsResult);

            return Unauthorized(chatsResult);
        }
        catch (Exception e)
        {
            return StatusCode(500, ReturnResult<List<Chat>>.Failed());
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReturnResult<Chat>>> GetChat(int id)
    {
        try
        {
            var chatsResult = await _chatManager.GetChatById(id);
            if (chatsResult.IsSuccess)
                return Ok(chatsResult);

            return Unauthorized(chatsResult);
        }
        catch (Exception e)
        {
            return StatusCode(500, ReturnResult<Chat>.Failed());
        }
    }
}
