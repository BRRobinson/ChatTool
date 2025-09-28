using AutoMapper;
using ChatTool.Database.Models;
using ChatTool.Models.DTOs;

namespace ChatTool.Mapper;

public static class UserMapper
{
    public static UserDTO ToDto(User user)
    {
        return new UserDTO
        {
            Id = user.Id,
            Username = user.Username,
        };
    }

    public static User ToEntity(UserDTO dto)
    {
        return new User
        {
            Id = dto.Id,
            Username = dto.Username,
        };
    }

    public static void UpdateEntity(User entity, UserDTO dto)
    {
        entity.Id = dto.Id;
        entity.Username = dto.Username;
    }
}

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>();

        CreateMap<UserDTO, User>();
    }
}