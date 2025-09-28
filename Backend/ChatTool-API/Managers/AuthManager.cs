using ChatTool.API.Interfaces;
using ChatTool.Database;
using ChatTool.Database.Models;
using ChatTool.Models;

namespace ChatTool.API.Managers;

public class AuthManager : IAuthManager
{
    private readonly ILogger<AuthManager> _logger;
    private readonly ITokenManager _tokenManager;
    private readonly DBContext _db;

    public AuthManager(ILogger<AuthManager> logger, ITokenManager tokenManager, DBContext db)
    {
        _logger = logger;
        _tokenManager = tokenManager;
        _db = db;
    }

    public ReturnResult<string> Login(LoginRequest loginRequest)
    {
        try
        {
            var user = _db.Users.SingleOrDefault(u => u.Username == loginRequest.Username);
            if (user == null)
                return ReturnResult<string>.Failed(default!, "Username could not be found.");

            if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
                return ReturnResult<string>.Failed(default!, "Username or Password incorrect.");

            return _tokenManager.CreateToken(user.Id, user.Username);
        }
        catch (Exception e)
        {

            throw;
        }
    }

    public ReturnResult<string> Register(LoginRequest loginRequest)
    {
        try
        {
            var user = _db.Users.SingleOrDefault(u => u.Username == loginRequest.Username);

            if (user != null)
                return ReturnResult<string>.Failed(default!, "User already exists with that Username.");

            _db.Users.Add(new User
            {
                Username = loginRequest.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(loginRequest.Password)
            });

            _db.SaveChanges();

            user = _db.Users.SingleOrDefault(u => u.Username == loginRequest.Username);

            return _tokenManager.CreateToken(user!.Id, user.Username);
        }
        catch (Exception e)
        {

            throw;
        }
    }
}
