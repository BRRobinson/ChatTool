using ChatTool.Models;
using ChatTool.Models.DTOs;

namespace ChatTool.API.Interfaces;

public interface IChatManager
{
    public Task<ReturnResult<List<ChatDTO>>> GetChats();

    public Task<ReturnResult<List<ChatDTO>>> GetUserChats(string user);

    public Task<ReturnResult<List<ChatDTO>>> GetUserChatsBytitle(string user, string title);

    public Task<ReturnResult<ChatDTO>> GetChatById(int id);

    public Task<ReturnResult<ChatDTO>> Insert(ChatDTO chat);

    public Task<ReturnResult<ChatDTO>> Update(ChatDTO chat);

    public Task<ReturnResult> Delete(int id);
}
