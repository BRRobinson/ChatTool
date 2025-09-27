using ChatTool.API.Models;
using ChatTool.Database.Models;

namespace ChatTool.API.Interfaces;

public interface IMessageManager
{
    public Task<ReturnResult<List<Message>>> GetMessages();

    public Task<ReturnResult<List<Message>>> GetMessagesBySender(string sender);

    public Task<ReturnResult<List<Message>>> GetMessagesByChat(int chatId);

    public Task<ReturnResult<List<Message>>> GetMessagesByChatSender(int chatId, string sender);

    public Task<ReturnResult<Message>> GetMessageById(int id);

    public Task<ReturnResult<Message>> Insert(Message message);

    public Task<ReturnResult<Message>> Update(Message message);

    public Task<ReturnResult<Message>> Delete(int id);
}
