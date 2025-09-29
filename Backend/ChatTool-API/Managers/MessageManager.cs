using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChatTool.API.Interfaces;
using ChatTool.Database;
using ChatTool.Database.Models;
using ChatTool.Models;
using ChatTool.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ChatTool.API.Managers;

public class MessageManager : IMessageManager
{
    private readonly ILogger<MessageManager> _logger;
    private readonly DBContext _db;
    private readonly IMapper _mapper;

    public MessageManager(ILogger<MessageManager> logger, DBContext db, IMapper mapper)
    {
        _logger = logger;
        _db = db;
        _mapper = mapper;
    }

    public async Task<ReturnResult<List<MessageDTO>>> GetMessages()
    {
        return ReturnResult<List<MessageDTO>>.Success(await _db.Messages.AsNoTracking().ProjectTo<MessageDTO>(_mapper.ConfigurationProvider).ToListAsync());
    }

    public async Task<ReturnResult<List<MessageDTO>>> GetMessagesByChat(int chatId)
    {
        var chatResult = await _db.Chats.FindAsync(chatId);
        if (chatResult == null)
            return ReturnResult<List<MessageDTO>>.Failed(null!, "Could not Find Chat for Messages.");

        return ReturnResult<List<MessageDTO>>.Success(await _db.Messages.AsNoTracking().Where(m => m.Chat.Id == chatId).ProjectTo<MessageDTO>(_mapper.ConfigurationProvider).ToListAsync());
    }

    public async Task<ReturnResult<List<MessageDTO>>> GetMessagesBySender(string sender)
    {
        var senderResult = await _db.Users.FirstOrDefaultAsync(u => u.Username == sender);
        if (senderResult == null)
            return ReturnResult<List<MessageDTO>>.Failed(null!, "Could not Find Sender for Messages.");

        return ReturnResult<List<MessageDTO>>.Success(await _db.Messages.AsNoTracking().Where(m => m.Sender.Id == senderResult.Id).ProjectTo<MessageDTO>(_mapper.ConfigurationProvider).ToListAsync());
    }

    public async Task<ReturnResult<List<MessageDTO>>> GetMessagesByChatSender(int chatId, string sender)
    {
        var chatResult = await _db.Chats.FindAsync(chatId);
        if (chatResult == null)
            return ReturnResult<List<MessageDTO>>.Failed(null!, "Could not Find Chat for Messages.");

        var senderResult = await _db.Users.FirstOrDefaultAsync(u => u.Username == sender);
        if (senderResult == null)
            return ReturnResult<List<MessageDTO>>.Failed(null!, "Could not Find Sender for Messages.");

        return ReturnResult<List<MessageDTO>>.Success(await _db.Messages.AsNoTracking().Where(m => m.Chat.Id == chatId && m.Sender.Id == senderResult.Id).ProjectTo<MessageDTO>(_mapper.ConfigurationProvider).ToListAsync());
    }

    public async Task<ReturnResult<MessageDTO>> GetMessageById(int id)
    {
        var messageResult = await _db.Messages.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        if (messageResult == null)
            return ReturnResult<MessageDTO>.Failed(null!, "Could not Find Message.");

        return ReturnResult<MessageDTO>.Success(_mapper.Map<MessageDTO>(messageResult!));
    }

    public async Task<ReturnResult<MessageDTO>> Insert(MessageDTO messageDto)
    {
        var validate = await ValidateMessage(messageDto);
        if (!validate.IsSuccess)
            return validate;

        var chat = await _db.Chats.FindAsync(messageDto.Chat.Id);
        if (chat == null)
            return ReturnResult<MessageDTO>.Failed(null!, "Could not Find Chat for Message.");

        var sender = await _db.Users.FindAsync(messageDto.Sender.Id);
        if (sender == null)
            return ReturnResult<MessageDTO>.Failed(null!, "Could not Find Sender for Message.");

        var messageEntity = _mapper.Map<Message>(messageDto);

        messageEntity.Chat = chat;
        messageEntity.Sender = sender;
        messageEntity.SentAt = DateTime.UtcNow;

        _db.Messages.Add(messageEntity);
        await _db.SaveChangesAsync();

        return ReturnResult<MessageDTO>.Success(_mapper.Map<MessageDTO>(messageEntity));
    }

    public async Task<ReturnResult<MessageDTO>> Update(MessageDTO messageDto)
    {
        var validate = await ValidateMessage(messageDto);
        if (!validate.IsSuccess)
            return validate;

        var messageEntity = await _db.Messages.FindAsync(messageDto.Id);

        if (messageEntity == null)
            throw new Exception("message not found");

        messageEntity.Content = messageDto.Content;
        messageEntity.IsEdited = true;

        await _db.SaveChangesAsync();

        return ReturnResult<MessageDTO>.Success(_mapper.Map<MessageDTO>(messageEntity));
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

    private async Task<ReturnResult<MessageDTO>> ValidateMessage(MessageDTO messageDto)
    {
        var chatResult = await _db.Chats.FindAsync(messageDto.Chat.Id);
        if (chatResult == null)
            return ReturnResult<MessageDTO>.Failed(null!, "Could not Find Chat for Message.");

        var senderResult = await _db.Users.FindAsync(messageDto.Sender.Id);
        if (senderResult == null)
            return ReturnResult<MessageDTO>.Failed(null!, "Could not Find Sender for Message.");

        if (string.IsNullOrWhiteSpace(messageDto.Content))
            return ReturnResult<MessageDTO>.Failed(null!, "Message content is null or empty.");

        return ReturnResult<MessageDTO>.Success(messageDto);
    }
}
