using ChatTool.API.Models;
using ChatTool.Database.Models;

namespace ChatTool.API.Interfaces;

public interface IChatManager
{
    public Task<ReturnResult<List<Chat>>> GetChats();

    public Task<ReturnResult<List<Chat>>> GetUserChats(string user);

    public Task<ReturnResult<List<Chat>>> GetUserChatsBytitle(string user, string title);

    public Task<ReturnResult<Chat>> GetChatById(int id);

    public Task<ReturnResult<Chat>> Insert(Chat chat);

    public Task<ReturnResult<Chat>> Update(Chat chat);

    public Task<ReturnResult<Chat>> Delete(int id);
}
