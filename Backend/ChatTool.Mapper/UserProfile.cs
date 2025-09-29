using AutoMapper;
using ChatTool.Database.Models;
using ChatTool.Models.DTOs;

namespace ChatTool.Mapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>();

        CreateMap<UserDTO, User>();
    }
}