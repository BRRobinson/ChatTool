using ChatTool.API.Interfaces;
using ChatTool.API.Models;
using ChatTool.Database;
using ChatTool.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatTool.API.Managers
{
    public class MessageManager : IMessageManager
    {
        private readonly ILogger<MessageManager> _logger;
        private readonly DBContext _db;

        public MessageManager(ILogger<MessageManager> logger, DBContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<ReturnResult<List<Message>>> GetMessages()
        {
            return ReturnResult<List<Message>>.Success(await _db.Messages.ToListAsync());
        }

        public async Task<ReturnResult<List<Message>>> GetMessagesByChat(int chatId)
        {
            var chatResult = await _db.Chats.FindAsync(chatId);
            if (chatResult == null)
                return ReturnResult<List<Message>>.Failed(null!, "Could not Find Chat for Messages.");

            return ReturnResult<List<Message>>.Success(await _db.Messages.Where(m => m.Chat.Id == chatId).ToListAsync());
        }

        public async Task<ReturnResult<List<Message>>> GetMessagesBySender(string sender)
        {
            var senderResult = await _db.Users.FirstOrDefaultAsync(u => u.Username == sender);
            if (senderResult == null)
                return ReturnResult<List<Message>>.Failed(null!, "Could not Find Sender for Messages.");

            return ReturnResult<List<Message>>.Success(await _db.Messages.Where(m => m.Sender.Id == senderResult.Id).ToListAsync());
        }

        public async Task<ReturnResult<List<Message>>> GetMessagesByChatSender(int chatId, string sender)
        {
            var chatResult = await _db.Chats.FindAsync(chatId);
            if (chatResult == null)
                return ReturnResult<List<Message>>.Failed(null!, "Could not Find Chat for Messages.");

            var senderResult = await _db.Users.FirstOrDefaultAsync(u => u.Username == sender);
            if (senderResult == null)
                return ReturnResult<List<Message>>.Failed(null!, "Could not Find Sender for Messages.");

            return ReturnResult<List<Message>>.Success(await _db.Messages.Where(m => m.Chat.Id == chatId && m.Sender.Id == senderResult.Id).ToListAsync());
        }

        public async Task<ReturnResult<Message>> GetMessageById(int id)
        {
            var messageResult = await _db.Messages.FindAsync(id);
            if (messageResult == null)
                return ReturnResult<Message>.Failed(null!, "Could not Find Message.");

            return ReturnResult<Message>.Success(messageResult!);
        }

        public async Task<ReturnResult<Message>> Insert(Message message)
        {
            var validate = await ValidateMessage(message);
            if (!validate.IsSuccess)
                return validate;

            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            return ReturnResult<Message>.Success(message);
        }

        public async Task<ReturnResult<Message>> Update(Message message)
        {
            var validate = await ValidateMessage(message);
            if (!validate.IsSuccess)
                return validate;


            var existingMessage = await _db.Messages.FindAsync(message.Id);

            if (existingMessage == null)
                return ReturnResult<Message>.Failed(null!, "Message not found");

            _db.Attach(message);

            _db.Entry(message).Property(m => m.message).IsModified = true;

            await _db.SaveChangesAsync();

            return ReturnResult<Message>.Success(message);
        }

        public async Task<ReturnResult> Delete(int id)
        {
            var messageResult = await _db.Messages.FindAsync(id);
            if (messageResult == null)
                return ReturnResult.Failed("Could not Find Message.");

            _db.Messages.Remove(messageResult);
            await _db.SaveChangesAsync();

            return ReturnResult.Success();
        }

        private async Task<ReturnResult<Message>> ValidateMessage(Message message)
        {
            var chatResult = await _db.Chats.FindAsync(message.Chat.Id);
            if (chatResult == null)
                return ReturnResult<Message>.Failed(null!, "Could not Find Chat for Message.");

            var senderResult = await _db.Users.FindAsync(message.Sender.Id);
            if (senderResult == null)
                return ReturnResult<Message>.Failed(null!, "Could not Find Sender for Message.");

            if (string.IsNullOrWhiteSpace(message.message))
                return ReturnResult<Message>.Failed(null!, "Message content is null or empty.");

            return ReturnResult<Message>.Success(message);
        }
    }
}
