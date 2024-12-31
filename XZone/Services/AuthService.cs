using AutoMapper;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using XZone.Models;
using XZone.Models.DTO.UserDto_s;

namespace XZone.Services
{
    public class AuthService : IAuthService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper mapper;
        private readonly IConfiguration config;

        public AuthService(RoleManager<IdentityRole > roleManager,UserManager<ApplicationUser> userManager, IMapper mapper,IConfiguration config)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
            this.mapper = mapper;
            this.config = config;
        }

       

        public async Task<ApiResponse> RegisterAsync(UserRegistrationDTO userRegistrationDTO)
        {

            if( await _userManager.FindByEmailAsync(userRegistrationDTO.Email) is not null)
            {

                return new ApiResponse { IsSuccess = false, ErrorMessages = { "Email is already Registered" } };
            }
            if(await _userManager.FindByNameAsync(userRegistrationDTO.UserName)is not null)
            {
                return new ApiResponse { IsSuccess = false, ErrorMessages = { "The User name is already Exist" } };
            }

            var user= mapper.Map<ApplicationUser>(userRegistrationDTO);

            var result= await _userManager.CreateAsync(user,userRegistrationDTO.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                {
                    errors += $"{error},";

                }
                return new ApiResponse { IsSuccess = false, ErrorMessages = { errors }  };
            

            }
            await _userManager.AddToRoleAsync(user, "User");
            var Token =await CreateTokenAsync(user);    

            return new ApiResponse { IsSuccess = true,Token= new JwtSecurityTokenHandler().WriteToken(Token) };
            
        }
        public async Task<JwtSecurityToken> CreateTokenAsync(ApplicationUser appUser)
        {

            List<Claim> UserClaims = new List<Claim>();

            UserClaims.Add(new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()));
            UserClaims.Add(new Claim(ClaimTypes.Name, appUser.FirstName.ToString()));
            UserClaims.Add(new Claim(ClaimTypes.Email, appUser.Email.ToString()));

            var Roles= await _userManager.GetRolesAsync(appUser);

            foreach (var role in Roles)
            {
                UserClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            UserClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));
            var SignCred=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
            (
                issuer: config["JWT:Issuer"],
                audience: config["JWT:Audience"],
                claims: UserClaims,
                signingCredentials: SignCred,
                expires: DateTime.Now.AddMinutes(1)

            );

            return jwtSecurityToken;
        }

        public async Task<ApiResponse> LoginAsync(UserLoginDTO userLoginDTO)
        {
            var UserInDb = await _userManager.FindByEmailAsync(userLoginDTO.Email);
            if (UserInDb == null || !(await _userManager.CheckPasswordAsync(UserInDb, userLoginDTO.Password)))
            {
                return new ApiResponse { ErrorMessages = { "The Password or Email is Wrong" }, IsSuccess = false };

            }
            var user = mapper.Map<ApplicationUser>(UserInDb);

            var Token= await CreateTokenAsync(user);

            return new ApiResponse { IsSuccess= true , Token = new JwtSecurityTokenHandler().WriteToken(Token) };


            }
        public async Task<string> AddRoleAsync(AddRoleDTO addRoleDTO)
        {
            var user = await _userManager.FindByIdAsync(addRoleDTO.UserId);

            if (user == null || !await _roleManager.RoleExistsAsync(addRoleDTO.Role))
            {
                return "Invalid User or Role";
            }

            if (await _userManager.IsInRoleAsync(user, addRoleDTO.Role))
            {
                return "User has the Role Already";
            }

            var result = await _userManager.AddToRoleAsync(user, addRoleDTO.Role);

            if (!result.Succeeded)
            {
                return "Some Shit happend ";

            }

            return string.Empty;



        }
        private RefreshToken CreateRefreshToken()
        {
            var RandomNumber = new byte[32];

            using var Generator = new RNGCryptoServiceProvider(RandomNumber);

            Generator.GetBytes(RandomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumber),
                ExpiresON = DateTime.UtcNow.AddHours(2),
                CreatedOn = DateTime.UtcNow,


            };

        }


        public async Task<ApiResponse> RefreshTokenAsync(string token)
        {
           var Response= new ApiResponse();

            var user = await _userManager.Users.SingleOrDefaultAsync(x=>x.RefreshToken.Any(x=>x.Token==token));
            if (user == null)
            {
                Response.IsSuccess = false;
                Response.ErrorMessages.Add( "Invaild Token");
                return Response;
            }
            var refreshtoken = user.RefreshToken.Single(x => x.Token == token);
            if (!refreshtoken.IsActive)
            {
                Response.IsSuccess = false;
                Response.ErrorMessages.Add("UnActive Token");
                return Response;
            }
            refreshtoken.RevokeOn = DateTime.UtcNow;
            var NewRefreshToken = CreateRefreshToken();
            user.RefreshToken.Add(refreshtoken);
            await _userManager.UpdateAsync(user);
            var NewJWT = await CreateTokenAsync(user);

            Response.IsSuccess = true;
            Response.Token = new JwtSecurityTokenHandler().WriteToken(NewJWT);
            Response.RefreshToken = NewRefreshToken.Token;
            Response.RefreshTokenExpiration = NewRefreshToken.ExpiresON;
            return Response;

        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.RefreshToken.Any(x => x.Token == token));
            if (user == null)
            {
                return false;
            }
            var refreshtoken = user.RefreshToken.Single(x => x.Token == token);
            if (!refreshtoken.IsActive)
            {
                return false;

            }

            refreshtoken.RevokeOn = DateTime.UtcNow;
            return true;
        }
    }

}
