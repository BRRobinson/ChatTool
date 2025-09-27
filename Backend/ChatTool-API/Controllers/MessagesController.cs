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
public class MessageController : ControllerBase
{
    private readonly ILogger<MessageController> _logger;
    private readonly IMessageManager _messageManager;

    public MessageController(ILogger<MessageController> logger, IMessageManager messageManager)
    {
        _logger = logger;
        _messageManager = messageManager;
    }

    [HttpGet("chat/{chatId:int}")]
    public async Task<ActionResult<ReturnResult<List<Message>>>> GetMessagesByChat(int chatId)
    {
        try
        {
            var messageResult = await _messageManager.GetMessagesByChat(chatId);
            if (messageResult.IsSuccess)
                return Ok(messageResult);

            return Unauthorized(messageResult);
        }
        catch (Exception e)
        {
            return StatusCode(500, ReturnResult<List<Message>>.Failed());
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReturnResult<Message>>> GetMessage(int id)
    {
        try
        {
            var messageResult = await _messageManager.GetMessageById(id);
            if (messageResult.IsSuccess)
                return Ok(messageResult);

            return Unauthorized(messageResult);
        }
        catch (Exception e)
        {
            return StatusCode(500, ReturnResult<Message>.Failed());
        }
    }

    [HttpPost]
    public async Task<ActionResult> PostMessage([FromBody] Message message)
    {
        try
        {
            var messageResult = await _messageManager.Insert(message);
            if (messageResult.IsSuccess)
                return Ok(messageResult);

            return Unauthorized(messageResult);
        }
        catch (Exception e)
        {
            return StatusCode(500, ReturnResult<string>.Failed());
        }
    }
}
