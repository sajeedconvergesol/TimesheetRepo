using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TMS.API.DTOs;
using TMS.API.Helpers;
using TMS.Core;
using TMS.Services.Interfaces;

namespace TMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _IUserService;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public LoginController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
           IUserService iUserService, IConfiguration config, IMapper mapper, ILogger<LoginController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _IUserService = iUserService;
            _config = config;
            _mapper = mapper;
            _logger = logger;
        }
        //Post : api/Login
        [HttpPost("SignIn")]
        public async Task<ResponseDTO<LoginResponseDTO>> SignIn([FromBody] LoginRequestDTO model)
        {
            ResponseDTO<LoginResponseDTO> response = new ResponseDTO<LoginResponseDTO>();
            int StatusCode = 0;
            bool isSuccess = false;
            LoginResponseDTO Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var user = await _IUserService.GetUserByEmail(model.Email);
                if (user != null)
                {
                    var role = await _IUserService.GetUserRole(user);

                    if (!UtilityHelper.IsValidEmail(model.Email))
                    {
                        isSuccess = false;
                        StatusCode = 400;
                        Message = "Invalid email address, Please provide valid email.";
                    }
                    else if (!user.IsActive)
                    {
                        isSuccess = false;
                        StatusCode = 400;
                        Message = "Account yet to be confirmed by Admin.";
                    }
                    else
                    {
                        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false,lockoutOnFailure: true);
                        if (result.Succeeded)
                        {
                            if (user != null)
                            {
                                var accessToken = GenerateJSONWebToken(user);
                                LoginResponseDTO userResponse = new LoginResponseDTO
                                {
                                    Token = accessToken,
                                    UserId = user.Id,
                                    FullName = user.FirstName + " " + user.LastName,
                                    UserRole = role[0].ToString(),
                                    Email = user.Email
                                };
                                Response = userResponse;
                                StatusCode = 200;
                                isSuccess = true;
                                Message = "SignIn successful.";
                                _logger.LogInformation("SignIn successful");
                            }
                            else
                            {
                                isSuccess = false;
                                StatusCode = 404;
                                Message = "User not found";
                            }
                        }
                        else if (result.IsLockedOut)
                        {
                            isSuccess = false;
                            StatusCode = 400;
                            Message = "SignIn disabled, please try again after 15 minutes";
                        }
                        else
                        {
                            if (user != null)
                            {
                                if (user.AccessFailedCount == 2)
                                {
                                    isSuccess = false;
                                    StatusCode = 400;
                                    Message = "Last attempt to login, if failed, please try again after 15 minutes";
                                }
                                else
                                {
                                    isSuccess = false;
                                    StatusCode = 400;
                                    Message = "Invalid username or password";
                                }
                            }
                            else
                            {
                                isSuccess = false;
                                StatusCode = 400;
                                Message = "Invalid username or password";
                            }
                        }
                    }
                }
                else
                {
                    isSuccess = false;
                    StatusCode = 404;
                    Message = "User not found";
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                StatusCode = 500;
                Message = "Failed while fetching data.";
                ExceptionMessage = ex.Message.ToString();
                _logger.LogError(ex.ToString(), ex);
            }

            response.StatusCode = StatusCode;
            response.IsSuccess = isSuccess;
            response.Response = Response;
            response.Message = Message;
            response.ExceptionMessage = ExceptionMessage;

            return response;
        }
        private string GenerateJSONWebToken(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(int.Parse(_config["Jwt:SessionTimeout"])),
            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
