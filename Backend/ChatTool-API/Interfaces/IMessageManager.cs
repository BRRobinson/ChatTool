using ChatTool.Models;
using ChatTool.Models.DTOs;

namespace ChatTool.API.Interfaces;

public interface IMessageManager
{
    public Task<ReturnResult<List<MessageDTO>>> GetMessages();

    public Task<ReturnResult<List<MessageDTO>>> GetMessagesBySender(string sender);

    public Task<ReturnResult<List<MessageDTO>>> GetMessagesByChat(int chatId);

    public Task<ReturnResult<List<MessageDTO>>> GetMessagesByChatSender(int chatId, string sender);

    public Task<ReturnResult<MessageDTO>> GetMessageById(int id);

    public Task<ReturnResult<MessageDTO>> Insert(MessageDTO message);

    public Task<ReturnResult<MessageDTO>> Update(MessageDTO message);

    public Task<ReturnResult> Delete(int id);
}
