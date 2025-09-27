using ChatTool.API.Models;
using ChatTool.Database.Models;

namespace ChatTool.API.Interfaces
{
    public interface IAuthManager
    {
        public ReturnResult<string> Login(LoginRequest loginRequest);

        public ReturnResult<string> Register(LoginRequest loginRequest);
    }
}
