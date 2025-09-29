using AutoMapper;
using ChatTool.Database.Models;
using ChatTool.Models.DTOs;

namespace ChatTool.Mapper;

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
