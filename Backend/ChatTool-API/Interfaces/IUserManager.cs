using ChatTool.Models;
using ChatTool.Models.DTOs;

namespace ChatTool.API.Interfaces;

public interface IUserManager
{
    public Task<ReturnResult<List<UserDTO>>> GetUsers();

    public Task<ReturnResult<UserDTO>> GetUserById(int id);

    public Task<ReturnResult<UserDTO>> GetUserByUsername(string username);

    public Task<ReturnResult<UserDTO>> Insert(UserDTO user);

    public Task<ReturnResult<UserDTO>> Update(UserDTO user);

    public Task<ReturnResult> Delete(int id);
}
