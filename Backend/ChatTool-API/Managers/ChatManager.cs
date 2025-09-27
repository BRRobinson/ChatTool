using ChatTool.API.Interfaces;
using ChatTool.API.Models;
using ChatTool.Database;
using ChatTool.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatTool.API.Managers
{
    public class ChatManager : IChatManager
    {
        private readonly ILogger<ChatManager> _logger;
        private readonly DBContext _db;

        public ChatManager(ILogger<ChatManager> logger, DBContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<ReturnResult<List<Chat>>> GetChats()
        {
            return ReturnResult<List<Chat>>.Success(await _db.Chats.ToListAsync());
        }

        public async Task<ReturnResult<List<Chat>>> GetUserChats(string user)
        {
            var userResult = await _db.Users.FirstOrDefaultAsync(u => u.Username == user);
            if (userResult == null)
                return ReturnResult<List<Chat>>.Failed(null!, "Could not Find User.");

            return ReturnResult<List<Chat>>.Success(await _db.Chats.Where(c => c.Participants.Contains(userResult)).ToListAsync());
        }

        public async Task<ReturnResult<List<Chat>>> GetUserChatsBytitle(string user, string title)
        {
            var userResult = await _db.Users.FirstOrDefaultAsync(u => u.Username == user);
            if (userResult == null)
                return ReturnResult<List<Chat>>.Failed(null!, "Could not Find User.");

            return ReturnResult<List<Chat>>.Success(await _db.Chats.Where(c => c.Participants.Contains(userResult) && c.Title.Contains(title)).ToListAsync());
        }

        public async Task<ReturnResult<Chat>> GetChatById(int id)
        {
            var chatResult = await _db.Chats.FindAsync(id);
            if (chatResult == null)
                return ReturnResult<Chat>.Failed(null!, "Could not Find Chat.");

            return ReturnResult<Chat>.Success(chatResult!);
        }

        public async Task<ReturnResult<Chat>> Insert(Chat chat)
        {
            var validate = await ValidateChat(chat);
            if (!validate.IsSuccess)
                return validate;

            _db.Chats.Add(chat);
            await _db.SaveChangesAsync();

            return ReturnResult<Chat>.Success(chat);
        }

        public async Task<ReturnResult<Chat>> Update(Chat chat)
        {
            var validate = await ValidateChat(chat);
            if (!validate.IsSuccess)
                return validate;

            var existingChat = await _db.Chats
                .Include(c => c.Participants)
                .AsTracking()
                .FirstOrDefaultAsync(c => c.Id == chat.Id);

            if (existingChat == null)
                return ReturnResult<Chat>.Failed(null!, "Chat not found");

            existingChat.Title = chat.Title;

            existingChat.Participants.Clear();
            foreach (var user in chat.Participants)
                existingChat.Participants.Add(user);

            await _db.SaveChangesAsync();

            return ReturnResult<Chat>.Success(chat);
        }

        public async Task<ReturnResult> Delete(int id)
        {
            var chatResult = await _db.Chats.FindAsync(id);
            if (chatResult == null)
                return ReturnResult<Chat>.Failed(null!, "Could not Find Chat.");
            _db.Chats.Remove(chatResult);
            await _db.SaveChangesAsync();
            return ReturnResult<Chat>.Success(chatResult);
        }

        private async Task<ReturnResult<Chat>> ValidateChat(Chat chat)
        {
            if (chat == null)
                return ReturnResult<Chat>.Failed(null!, "Chat is null.");

            if (string.IsNullOrEmpty(chat.Title))
                return ReturnResult<Chat>.Failed(null!, "Chat Title is null or empty.");

            if (chat.Participants == null || chat.Participants.Count < 2)
                return ReturnResult<Chat>.Failed(null!, "Chat must have at least 2 participant.");

            foreach (var user in chat.Participants)
            {
                var userResult = await _db.Users.FirstOrDefaultAsync(u => u.Username == user.Username && u.Id == user.Id);
                if (userResult == null)
                    return ReturnResult<Chat>.Failed(null!, $"Could not Find User: {user.Username}");
            }

            return ReturnResult<Chat>.Success(chat);
        }
    }
}
