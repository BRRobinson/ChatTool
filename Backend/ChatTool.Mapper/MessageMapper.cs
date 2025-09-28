using AutoMapper;
using ChatTool.Database.Models;
using ChatTool.Models.DTOs;

namespace ChatTool.Mapper;

public static class MessageMapper
{
    public static MessageDTO ToDto(Message Message)
    {
        return new MessageDTO
        {
            Id = Message.Id,
            Chat = ChatMapper.ToDto(Message.Chat),
            Sender = UserMapper.ToDto(Message.Sender),
            SentAt = Message.SentAt,
            message = Message.message,
        };
    }

    public static Message ToEntity(MessageDTO dto)
    {
        return new Message
        {
            Id = dto.Id,
            Chat = ChatMapper.ToEntity(dto.Chat),
            Sender = UserMapper.ToEntity(dto.Sender),
            SentAt = dto.SentAt,
            message = dto.message,
        };
    }

    public static void UpdateEntity(Message entity, MessageDTO dto)
    {
        entity.Id = dto.Id;
        entity.Chat = ChatMapper.ToEntity(dto.Chat);
        entity.Sender = UserMapper.ToEntity(dto.Sender);
        entity.SentAt = dto.SentAt;
        entity.message = dto.message;
    }
}

public class MessageProfile : Profile
{
    public MessageProfile()
    {
        // Entity -> DTO
        CreateMap<Message, MessageDTO>()
            .ForMember(dest => dest.Chat, opt => opt.MapFrom(src => src.Chat))
            .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.Sender));

        // DTO -> Entity
        CreateMap<MessageDTO, Message>()
            .ForMember(dest => dest.Chat, opt => opt.MapFrom(src => src.Chat))
            .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.Sender));
    }
}