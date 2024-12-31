using System.IdentityModel.Tokens.Jwt;
using XZone.Models;
using XZone.Models.DTO.UserDto_s;

namespace XZone.Services
{
    public interface IAuthService
    {
        public Task<ApiResponse> RegisterAsync(UserRegistrationDTO userRegistrationDTO);

         Task<JwtSecurityToken> CreateTokenAsync(ApplicationUser appUser);
        Task<ApiResponse> LoginAsync(UserLoginDTO userLoginDTO );
        public Task<string> AddRoleAsync(AddRoleDTO addRoleDTO);
        Task<ApiResponse> RefreshTokenAsync(string token);

        Task<bool> RevokeTokenAsync(string token);




    }
}
