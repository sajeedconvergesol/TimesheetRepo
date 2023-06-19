﻿using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TMS.API.DTOs;
using TMS.API.Helpers;
using TMS.Core;
using TMS.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserService _IUserService;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
           IUserService iUserService, IConfiguration config, IMapper mapper, ILogger<AccountController> logger, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _IUserService = iUserService;
            _config = config;
            _mapper = mapper;
            _logger = logger;
            _roleManager = roleManager;
        }
        //Post : api/Account
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
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false,lockoutOnFailure: true);
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
                 new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(int.Parse(_config["Jwt:SessionTimeout"])),
            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("Registration")]
        public async Task<ResponseDTO<ApplicationUser>> Registration(ApplicationUser newUser)
        {
            ResponseDTO<ApplicationUser> response = new ResponseDTO<ApplicationUser>();
            int StatusCode = 0;
            bool isSuccess = false;
            LoginResponseDTO Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                IdentityResult result = await _IUserService.CreateAsync(newUser, newUser.PasswordHash, "Developers");
                if(!result.Succeeded)
                {
                    var er = "";
                    foreach (var error in result.Errors)
                    {
                        er += " " + error.Description+" [+] ";
                    }
                    isSuccess = false;
                    StatusCode = 400;
                    Message = er;
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Account Created.";
                    _logger.LogInformation("Account Created successful");
                }
            }
            catch (Exception error)
            {
                isSuccess = false;
                StatusCode = 500;
                Message = "Failed while Creating data.";
                ExceptionMessage = error.Message.ToString();
                _logger.LogError(error.ToString(), error);
            }
            response.StatusCode = StatusCode;
            response.IsSuccess = isSuccess;
            response.Message = Message;
            response.ExceptionMessage = ExceptionMessage;
            return response;
        }

        [HttpGet("SignOut")]
        public async Task<ResponseDTO<string>> SignOut()
        {
            ResponseDTO<string> response = new ResponseDTO<string>();
            int StatusCode = 0;
            bool isSuccess = false;
            LoginResponseDTO Response = null;
            string Message = "";
            string ExceptionMessage = "";
            if (User.Identity.Name !=null)
            { 
                _signInManager.SignOutAsync();
                StatusCode = 200;
                isSuccess = true;
                Message = "Account Logout successful.";
                ExceptionMessage = User.Identity.Name;
                _logger.LogInformation("Account Logout successful");
            }
            else
            {
                isSuccess = false;
                StatusCode = 500;
                Message = "Failed while Account LogOut. Please Login First";                
            }

            response.StatusCode = StatusCode;
            response.IsSuccess = isSuccess;
            response.Message = Message;
            response.ExceptionMessage = ExceptionMessage;
            return response;
        }

        [HttpGet("AccountDetails")]
        public async Task<ResponseDTO<ApplicationUser>> GetUserLoginInformation(string userId)
        {
            ResponseDTO<ApplicationUser> response = new ResponseDTO<ApplicationUser>();
            int StatusCode = 0;
            bool isSuccess = false;
            ApplicationUser Response = null;
            string Message = "";
            string ExceptionMessage = "";

            try
            {
                ApplicationUser user = await _IUserService.GetById(userId);
                if (user != null)
                {
                    Response = user;
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Accound Details Found";
                    _logger.LogInformation("Accound Details Found");
                }
                else
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Invalid userId";
                }
            }
            catch (Exception error)
            {
                isSuccess = false;
                StatusCode = 500;
                Message = "Failed while Fetching data.";
                ExceptionMessage = error.Message.ToString();
                _logger.LogError(error.ToString(), error);
            }
            response.StatusCode = StatusCode;
            response.IsSuccess = isSuccess;
            response.Message = Message;
            response.Response = Response;
            response.ExceptionMessage = ExceptionMessage;
            return response;
        }

        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<ResponseDTO<string>> ChangePassword(ChangePasswordDTO vmChangePassword)
        {
            ResponseDTO<string> response = new ResponseDTO<string>();
            int StatusCode = 0;
            bool isSuccess = false;
            LoginResponseDTO Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var result = await _IUserService.ChangePasswordAsync(vmChangePassword.Email, vmChangePassword.CurrentPassword, vmChangePassword.NewPassword, vmChangePassword.ConfirmPassword);
                
                
                if (!result)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "possible errors: -Email Does not Exists -Current Password Does not match";
                }else
                {
                    isSuccess = true;
                    StatusCode = 200;
                    Message = "New Password Created";
                }
            }
            catch (Exception error)
            {
                isSuccess = false;
                StatusCode = 500;
                Message = "Unable to create new Password";
                ExceptionMessage = error.Message.ToString();
                _logger.LogError(error.ToString(), error);
            }
            response.StatusCode = StatusCode;
            response.IsSuccess = isSuccess;
            response.Message = Message;
            response.ExceptionMessage = ExceptionMessage;
            return response;
        }

        //not working right now 
        [HttpPut("UpdateUser")]
        [Authorize]
        public async Task<ResponseDTO<string>> UpdateUser(UpdateUserDTO newUser)
        {
            ResponseDTO<string> response = new ResponseDTO<string>();
            int StatusCode = 0;
            bool isSuccess = false;
            string Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var user = await _IUserService.GetByUserName(User.Identity.Name);
                if(user != null) 
                {
                    ApplicationUser UpdatedUser = new ApplicationUser()
                    {
                        FirstName = newUser.FirstName,
                        LastName = newUser.LastName,
                        PostalCode = newUser.PostalCode,
                        MobileNo = newUser.MobileNumber,
                        Address1 = newUser.Address1,
                        Address2 = newUser.Address2,
                        City = newUser.City,
                        State = newUser.State,
                        Country = newUser.Country,
                        Gender = newUser.Gender,
                        ManagerId = newUser.ManagerId,
                        PhoneNumber = newUser.PhoneNumber,
                    };
                    IdentityResult result = await _IUserService.UpdateUser(UpdatedUser);
                    if(!result.Succeeded)
                    {
                        var er = "";
                        foreach (var error in result.Errors)
                        {
                            er += " " + error.Description + " [+] ";
                        }
                        isSuccess = false;
                        StatusCode = 400;
                        Message = er;
                    }
                    else
                    {
                        isSuccess = true;
                        StatusCode = 200;
                        Message = "Updated User successfully";
                    }
                }
            }
            catch (Exception error)
            {
                isSuccess = false;
                StatusCode = 500;
                Message = "Please Login First";
                ExceptionMessage = error.Message.ToString();
                _logger.LogError(error.ToString(), error);
            }
            response.StatusCode = StatusCode;
            response.IsSuccess = isSuccess;
            response.Message = Message;
            response.ExceptionMessage = ExceptionMessage;
            return response;
        }

        [HttpPost("ResetPassword")]
        [Authorize]
        public async Task<ResponseDTO<string>> ResetPassword(ResetPasswordDTO resetPass)
        {
            ResponseDTO<string> response = new ResponseDTO<string>();
            int StatusCode = 0;
            bool isSuccess = false;
            LoginResponseDTO Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var user = await _IUserService.GetUserByEmail(resetPass.Email);
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _IUserService.ResetPasswordAsync(user, code, resetPass.NewPassword);
                if(result)
                {
                    isSuccess = false;
                    StatusCode = 200;
                    Message = "Password Changed completed successfully";
                }
                else
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Not Changes";
                }
            }
            catch (Exception error)
            {
                isSuccess = false;
                StatusCode = 500;
                Message = "Unable to Reset Password";
                ExceptionMessage = error.Message.ToString();
                _logger.LogError(error.ToString(), error);
            }
            response.StatusCode = StatusCode;
            response.IsSuccess = isSuccess;
            response.Message = Message;
            response.ExceptionMessage = ExceptionMessage;
            return response;
        }
        [HttpPost("AccountUpdate")]
        [Authorize]
        public async Task<ResponseDTO<ApplicationUser>> AccountUpdate(ApplicationUser updateUser)
        {
            ResponseDTO<ApplicationUser> response = new ResponseDTO<ApplicationUser>();
            int StatusCode = 0;
            bool isSuccess = false;
            ApplicationUser Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                IdentityResult result = await _IUserService.UpdateUser(updateUser);
                if (!result.Succeeded)
                {
                    var er = "";
                    foreach (var error in result.Errors)
                    {
                        er += " " + error.Description + " [+] ";
                    }
                    isSuccess = false;
                    StatusCode = 400;
                    Message = er;
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Account Created successful.";
                    _logger.LogInformation("Account Created successful");
                }
            }
            catch (Exception error)
            {
                isSuccess = false;
                StatusCode = 500;
                Message = "Failed while Updating data.";
                ExceptionMessage = error.Message.ToString();
                _logger.LogError(error.ToString(), error);
            }
        
            response.StatusCode = StatusCode;
            response.IsSuccess = isSuccess;
            response.Message = Message;
            response.ExceptionMessage = ExceptionMessage;
            return response;
        }
    }
}