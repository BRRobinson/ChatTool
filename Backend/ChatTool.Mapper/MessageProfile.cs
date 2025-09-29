using AutoMapper;
using ChatTool.Database.Models;
using ChatTool.Models.DTOs;

namespace ChatTool.Mapper;

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