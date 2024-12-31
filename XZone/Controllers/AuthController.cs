using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using XZone.Models;
using XZone.Models.DTO.UserDto_s;
using XZone.Services;

namespace XZone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private ApiResponse _response;
        public AuthController(IAuthService authService)
        {
            this.authService = authService;
            this._response = new ApiResponse();
        }

        [HttpPost("Register")]

        public async Task<ActionResult<ApiResponse>> Register(UserRegistrationDTO userRegistrationDTO)
        {
            if (!ModelState.IsValid)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                var errors = ModelState.Values
             .SelectMany(v => v.Errors)
             .Select(e => e.ErrorMessage)
             .ToList();
                _response.ErrorMessages = errors;
                return BadRequest(_response);


            }

            var Result = await authService.RegisterAsync(userRegistrationDTO);

            if (!Result.IsSuccess)
            {
                Result.StatusCode = HttpStatusCode.BadRequest;

                return BadRequest(Result);

            }
            Result.StatusCode = HttpStatusCode.Created;
            return Ok(Result);

        }

        [HttpPost("Login")]

        public async Task<ActionResult<ApiResponse>> Login(UserLoginDTO userLoginDTO)
        {
            if (!ModelState.IsValid)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                var errors = ModelState.Values
             .SelectMany(v => v.Errors)
             .Select(e => e.ErrorMessage)
             .ToList();
                _response.ErrorMessages = errors;
                return BadRequest(_response);
            }

            _response = await authService.LoginAsync(userLoginDTO);
            if (!_response.IsSuccess)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);

            }
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);



        }

        [HttpPost("AddRole")]

        public async Task<ActionResult<ApiResponse>> AddRole(AddRoleDTO addRoleDTO)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                var errors = ModelState.Values
             .SelectMany(v => v.Errors)
             .Select(e => e.ErrorMessage)
             .ToList();
                _response.ErrorMessages = errors;
                return BadRequest(_response);

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await authService.AddRoleAsync(addRoleDTO);
            if (!String.IsNullOrEmpty(result))
            {
                return BadRequest(result);
            }

            return Ok(addRoleDTO);

        }

        [HttpGet("GetRefreshToken")]

        public async Task<ActionResult<ApiResponse>> RefreshToken()
        {
            var refreshtoken = Request.Cookies["RefreshToken"];
            var Result = await authService.RefreshTokenAsync(refreshtoken);
            if (!Result.IsSuccess)
            {
                return BadRequest(Result.ErrorMessages);
            }
            SetRefreshTokenInCookie(Result.RefreshToken, Result.RefreshTokenExpiration);
            return Ok(Result);
        }

        [HttpPost("RevokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] TokenRevokeDTO tokenRevokeDTO)
        {

            var RefreshToken = tokenRevokeDTO.Token ?? Request.Cookies["RefreshToken"];

            if (String.IsNullOrEmpty(RefreshToken))
            {
                return BadRequest("Invalid Token");
            }
            var Result = await authService.RevokeTokenAsync(RefreshToken);
            if (!Result)
            {
                return BadRequest("Token is Invalid");
            }
            return Ok();

        }
        private void SetRefreshTokenInCookie(string refreshToken, DateTime ExpiresOn)
        {
            var cookieoptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = ExpiresOn.ToLocalTime(),

            };

            Response.Cookies.Append("RefreshToken", refreshToken, cookieoptions);

        }
    }
}