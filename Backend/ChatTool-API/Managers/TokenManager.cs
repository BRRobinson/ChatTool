using ChatTool.API.Interfaces;
using ChatTool.API.Models;
using ChatTool.API.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatTool.API.Managers
{
    public class TokenManager : ITokenManager
    {
        private readonly AppSettings appSettings;
        private readonly ILogger<TokenManager> logger;

        public TokenManager(ILogger<TokenManager> logger, IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
            this.logger = logger;
        }
        public ReturnResult<string> CreateToken(int userId, string userName)
        {
            try
            {
                var claims = new[]
                {
                    new Claim("id", userId.ToString()),
                    new Claim("username", userName),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JWT.Key));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: appSettings.JWT.Issuer,
                    audience: appSettings.JWT.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds);

                return ReturnResult<string>.Success(new JwtSecurityTokenHandler().WriteToken(token));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error creating token for user {UserId}", userId);
                return ReturnResult<string>.Failed(string.Empty, "Error creating token");
            }
        }

        public ReturnResult<int> ValidateToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
