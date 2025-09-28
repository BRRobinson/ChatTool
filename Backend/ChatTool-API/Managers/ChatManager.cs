using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChatTool.API.Interfaces;
using ChatTool.Database;
using ChatTool.Database.Models;
using ChatTool.Mapper;
using ChatTool.Models;
using ChatTool.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ChatTool.API.Managers;

public class ChatManager : IChatManager
{
    private readonly ILogger<ChatManager> _logger;
    private readonly DBContext _db;
    private readonly IMapper _mapper;

    public ChatManager(ILogger<ChatManager> logger, DBContext db, IMapper mapper)
    {
        _logger = logger;
        _db = db;
        _mapper = mapper;
    }

    public async Task<ReturnResult<List<ChatDTO>>> GetChats()
    {
        return ReturnResult<List<ChatDTO>>.Success(await _db.Chats.ProjectTo<ChatDTO>(_mapper.ConfigurationProvider).ToListAsync());
    }

    public async Task<ReturnResult<List<ChatDTO>>> GetUserChats(string user)
    {
        var userResult = await _db.Users.FirstOrDefaultAsync(u => u.Username == user);
        if (userResult == null)
            return ReturnResult<List<ChatDTO>>.Failed(null!, "Could not Find User.");

        return ReturnResult<List<ChatDTO>>.Success(await _db.Chats.Where(c => c.Participants.Contains(userResult)).ProjectTo<ChatDTO>(_mapper.ConfigurationProvider).ToListAsync());
    }

    public async Task<ReturnResult<List<ChatDTO>>> GetUserChatsBytitle(string user, string title)
    {
        var userResult = await _db.Users.FirstOrDefaultAsync(u => u.Username == user);
        if (userResult == null)
            return ReturnResult<List<ChatDTO>>.Failed(null!, "Could not Find User.");

        return ReturnResult<List<ChatDTO>>.Success(await _db.Chats.Where(c => c.Participants.Contains(userResult) && c.Title.Contains(title)).ProjectTo<ChatDTO>(_mapper.ConfigurationProvider).ToListAsync());
    }

    public async Task<ReturnResult<ChatDTO>> GetChatById(int id)
    {
        var chatResult = await _db.Chats.FindAsync(id);
        if (chatResult == null)
            return ReturnResult<ChatDTO>.Failed(null!, "Could not Find Chat.");

        return ReturnResult<ChatDTO>.Success(_mapper.Map<ChatDTO>(chatResult!));
    }

    public async Task<ReturnResult<ChatDTO>> Insert(ChatDTO chatDto)
    {
        var validate = await ValidateChat(chatDto);
        if (!validate.IsSuccess)
            return validate;

        var participantIds = chatDto.Participants.Select(p => p.Id).ToList();
        var participants = await _db.Users
            .Where(u => participantIds.Contains(u.Id))
            .ToListAsync();

        var chatEntity = _mapper.Map<Chat>(chatDto);

        chatEntity.Participants = participants;

        _db.Chats.Add(chatEntity);
        await _db.SaveChangesAsync();

        return ReturnResult<ChatDTO>.Success(_mapper.Map<ChatDTO>(chatEntity));
    }

    public async Task<ReturnResult<ChatDTO>> Update(ChatDTO chatDto)
    {
        var validate = await ValidateChat(chatDto);
        if (!validate.IsSuccess)
            return validate;
        
        var chatEntity = await _db.Chats.FindAsync(chatDto.Id);

        if (chatEntity == null)
            throw new Exception("Chat not found");

        _mapper.Map(chatDto, chatEntity);

        chatEntity.Participants.Clear();
        var participantIds = chatDto.Participants.Select(p => p.Id).ToList();
        var participants = await _db.Users
            .Where(u => participantIds.Contains(u.Id))
            .ToListAsync();
        chatEntity.Participants.AddRange(participants);

        _db.Chats.Update(chatEntity);

        await _db.SaveChangesAsync();

        return ReturnResult<ChatDTO>.Success(_mapper.Map<ChatDTO>(chatEntity));
    }

    public async Task<ReturnResult> Delete(int id)
    {
        var chatResult = await _db.Chats.FindAsync(id);
        if (chatResult == null)
            return ReturnResult<Chat>.Failed(null!, "Could not Find Chat.");
        _db.Chats.Remove(chatResult);
        await _db.SaveChangesAsync();
        return ReturnResult.Success();
    }

    private async Task<ReturnResult<ChatDTO>> ValidateChat(ChatDTO chatDto)
    {
        if (chatDto == null)
            return ReturnResult<ChatDTO>.Failed(null!, "Chat is null.");

        if (string.IsNullOrEmpty(chatDto.Title))
            return ReturnResult<ChatDTO>.Failed(null!, "Chat Title is null or empty.");

        if (chatDto.Participants == null || chatDto.Participants.Count < 2)
            return ReturnResult<ChatDTO>.Failed(null!, "Chat must have at least 2 participant.");

        foreach (var user in chatDto.Participants)
        {
            var userResult = await _db.Users.FirstOrDefaultAsync(u => u.Username == user.Username && u.Id == user.Id);
            if (userResult == null)
                return ReturnResult<ChatDTO>.Failed(null!, $"Could not Find User: {user.Username}");
        }

        return ReturnResult<ChatDTO>.Success(chatDto);
    }
}
