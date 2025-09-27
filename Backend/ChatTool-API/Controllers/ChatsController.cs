using ChatTool.API.Interfaces;
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

            return BadRequest(chatsResult);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
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

            return BadRequest(chatsResult);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, ReturnResult<Chat>.Failed());
        }
    }

    [HttpPost]
    public async Task<ActionResult<ReturnResult<Chat>>> CreateChat([FromBody] Chat chat)
    {
        try
        {
            var chatsResult = await _chatManager.Insert(chat);
            if (chatsResult.IsSuccess)
                return Ok(chatsResult);

            return BadRequest(chatsResult);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, ReturnResult<Chat>.Failed());
        }
    }

    [HttpPatch]
    public async Task<ActionResult<ReturnResult<Chat>>> AlterChat([FromBody] Chat chat)
    {
        try
        {
            var chatsResult = await _chatManager.Update(chat);
            if (chatsResult.IsSuccess)
                return Ok(chatsResult);

            return BadRequest(chatsResult);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, ReturnResult<Chat>.Failed());
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ReturnResult>> DeleteChat(int id)
    {
        try
        {
            var chatsResult = await _chatManager.Delete(id);
            if (chatsResult.IsSuccess)
                return Ok(chatsResult);

            return BadRequest(chatsResult);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, ReturnResult.Failed());
        }
    }
}
