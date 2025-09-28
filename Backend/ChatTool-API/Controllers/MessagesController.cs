using ChatTool.API.Interfaces;
using ChatTool.Models;
using ChatTool.Models.DTOs;
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
    public async Task<ActionResult<ReturnResult<List<MessageDTO>>>> GetMessagesByChat(int chatId)
    {
        try
        {
            var messageResult = await _messageManager.GetMessagesByChat(chatId);
            if (messageResult.IsSuccess)
                return Ok(messageResult);

            return BadRequest(messageResult);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, ReturnResult<List<MessageDTO>>.Failed());
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReturnResult<MessageDTO>>> GetMessage(int id)
    {
        try
        {
            var messageResult = await _messageManager.GetMessageById(id);
            if (messageResult.IsSuccess)
                return Ok(messageResult);

            return BadRequest(messageResult);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, ReturnResult<MessageDTO>.Failed());
        }
    }

    [HttpPost]
    public async Task<ActionResult> PostMessage([FromBody] MessageDTO message)
    {
        try
        {
            var messageResult = await _messageManager.Insert(message);
            if (messageResult.IsSuccess)
                return Ok(messageResult);

            return BadRequest(messageResult);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, ReturnResult<string>.Failed());
        }
    }

    [HttpPatch]
    public async Task<ActionResult> UpdateMessage([FromBody] MessageDTO message)
    {
        try
        {
            var messageResult = await _messageManager.Update(message);
            if (messageResult.IsSuccess)
                return Ok(messageResult);

            return BadRequest(messageResult);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, ReturnResult<string>.Failed());
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteMessage(int id)
    {
        try
        {
            var messageResult = await _messageManager.Delete(id);
            if (messageResult.IsSuccess)
                return Ok(messageResult);
            
            return BadRequest(messageResult);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, ReturnResult<string>.Failed());
        }
    }
}
