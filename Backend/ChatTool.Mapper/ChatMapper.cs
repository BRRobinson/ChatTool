using AutoMapper;
using ChatTool.Database.Models;
using ChatTool.Models.DTOs;

namespace ChatTool.Mapper;

public static class ChatMapper
{
    public static ChatDTO ToDto(Chat Chat)
    {
        return new ChatDTO
        {
            Id = Chat.Id,
            Title = Chat.Title,
            Participants = Chat.Participants.Select(UserMapper.ToDto).ToList()
        };
    }

    public static Chat ToEntity(ChatDTO dto)
    {
        return new Chat
        {
            Id = dto.Id,
            Title = dto.Title,
            Participants = dto.Participants.Select(UserMapper.ToEntity).ToList()
        };
    }

    public static void UpdateEntity(Chat entity, ChatDTO dto)
    {
        entity.Id = dto.Id;
        entity.Title = dto.Title;
        entity.Participants = dto.Participants.Select(UserMapper.ToEntity).ToList();
    }
}
public class ChatProfile : Profile
{
    public ChatProfile()
    {
        CreateMap<Chat, ChatDTO>()
            .ForMember(dest => dest.Participants,
                       opt => opt.MapFrom(src => src.Participants));

        CreateMap<ChatDTO, Chat>()
            .ForMember(dest => dest.Participants,
                       opt => opt.MapFrom(src => src.Participants));
    }
}
