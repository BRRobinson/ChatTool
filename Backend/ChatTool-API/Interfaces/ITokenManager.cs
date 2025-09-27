using ChatTool.API.Models;

namespace ChatTool.API.Interfaces
{
    public interface ITokenManager
    {
        public ReturnResult<string> CreateToken(int userId, string userName);
        public ReturnResult<int> ValidateToken(string token);
    }
}
