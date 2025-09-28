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

public class UserManager : IUserManager
{
    private readonly ILogger<UserManager> _logger;
    private readonly DBContext _db;
    private readonly IMapper _mapper;

    public UserManager(ILogger<UserManager> logger, DBContext db, IMapper mapper)
    {
        _logger = logger;
        _db = db;
        _mapper = mapper;
    }

    public async Task<ReturnResult<List<UserDTO>>> GetUsers()
    {
        return ReturnResult<List<UserDTO>>.Success(await _db.Users.ProjectTo<UserDTO>(_mapper.ConfigurationProvider).ToListAsync());
    }

    public async Task<ReturnResult<UserDTO>> GetUserById(int id)
    {
        var userResult = await _db.Users.FindAsync(id);
        if (userResult == null)
            return ReturnResult<UserDTO>.Failed(null!, "Could not Find User.");

        return ReturnResult<UserDTO>.Success(_mapper.Map<UserDTO>(userResult!));
    }

    public async Task<ReturnResult<UserDTO>> GetUserByUsername(string username)
    {
        var userResult = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (userResult == null)
            return ReturnResult<UserDTO>.Failed(null!, "Could not Find User.");

        return ReturnResult<UserDTO>.Success(_mapper.Map<UserDTO>(userResult!));
    }

    public Task<ReturnResult<UserDTO>> Insert(UserDTO user)
    {
        throw new NotImplementedException();
    }

    public Task<ReturnResult<UserDTO>> Update(UserDTO user)
    {
        throw new NotImplementedException();
    }

    public Task<ReturnResult> Delete(int id)
    {
        throw new NotImplementedException();
    }
}
