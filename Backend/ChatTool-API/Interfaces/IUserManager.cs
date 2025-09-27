using ChatTool.API.Models;
using ChatTool.Database.Models;

namespace ChatTool.API.Interfaces;

public interface IUserManager
{
    public Task<ReturnResult<List<User>>> GetUsers();

    public Task<ReturnResult<User>> GetUserById(int id);

    public Task<ReturnResult<User>> GetUserByUsername(string username);

    public Task<ReturnResult<User>> Insert(User user);

    public Task<ReturnResult<User>> Update(User user);

    public Task<ReturnResult> Delete(int id);
}
