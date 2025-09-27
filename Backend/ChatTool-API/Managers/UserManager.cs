using ChatTool.API.Interfaces;
using ChatTool.API.Models;
using ChatTool.Database;
using ChatTool.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatTool.API.Managers;

public class UserManager : IUserManager
{
    private readonly ILogger<UserManager> _logger;
    private readonly DBContext _db;

    public UserManager(ILogger<UserManager> logger, DBContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<ReturnResult<List<User>>> GetUsers()
    {
        return ReturnResult<List<User>>.Success(await _db.Users.ToListAsync());
    }

    public async Task<ReturnResult<User>> GetUserById(int id)
    {
        var userResult = await _db.Users.FindAsync(id);
        if (userResult == null)
            return ReturnResult<User>.Failed(null!, "Could not Find User.");

        return ReturnResult<User>.Success(userResult!);
    }

    public async Task<ReturnResult<User>> GetUserByUsername(string username)
    {
        var userResult = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (userResult == null)
            return ReturnResult<User>.Failed(null!, "Could not Find User.");

        return ReturnResult<User>.Success(userResult!);
    }

    public Task<ReturnResult<User>> Insert(User user)
    {
        throw new NotImplementedException();
    }

    public Task<ReturnResult<User>> Update(User user)
    {
        throw new NotImplementedException();
    }

    public Task<ReturnResult> Delete(int id)
    {
        throw new NotImplementedException();
    }
}
