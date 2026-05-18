using HRMS.Core.DTOs;

namespace HRMS.Core.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(UserRegisterDto userRegisterDto);
        Task<string> LoginAsync(UserLoginDto userLoginDto);
    }
}
