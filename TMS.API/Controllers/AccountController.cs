using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TMS.API.DTOs;
using TMS.API.Helpers;
using TMS.Core;
using TMS.Infrastructure.Interfaces;
using TMS.Services.Interfaces;
using TMS.Services.Services;

namespace TMS.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        private readonly IUserResolverService _userResolverService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMailService _mailService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
           IUserService iUserService, IConfiguration config,
           IMapper mapper, ILogger<AccountController> logger,
           RoleManager<ApplicationRole> roleManager, IUserResolverService userResolverService,
           IWebHostEnvironment webHostEnvironment, IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _IUserService = iUserService;
            _config = config;
            _mapper = mapper;
            _logger = logger;
            _roleManager = roleManager;
            _userResolverService = userResolverService;
            _webHostEnvironment = webHostEnvironment;
            _mailService = mailService;
        }

        #region UserSignIn
        //Post : api/Account
        [AllowAnonymous]
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
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: true);
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

        #endregion

        #region GenerateToken
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


        #endregion

        #region UserRegistration
        [HttpPost("Registration")]
        public async Task<ResponseDTO<ApplicationUser>> Registration(RequestUserDTO newUser)
        {
            ResponseDTO<ApplicationUser> response = new ResponseDTO<ApplicationUser>();
            int StatusCode = 0;
            bool isSuccess = false;
            LoginResponseDTO Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var user = _mapper.Map<ApplicationUser>(newUser);
                var currentUser = await _userResolverService.GetCurrentUser();
                user.CreatedBy = currentUser.Id;
                user.CreatedOn = DateTime.UtcNow;
                user.UpdatedBy = currentUser.Id;
                user.UpdatedOn = DateTime.UtcNow;
                user.EmailConfirmed = true;
                user.IsActive = true;
                user.PhoneNumberConfirmed = true;
                user.SecurityStamp = Guid.NewGuid().ToString("D");
                IdentityResult result = await _IUserService.CreateAsync(user, newUser.Password, newUser.Role);
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
                    Message = "Account Created.";
                    _logger.LogInformation("Account Created successful");

                    //send Email That account has been Created
                    var email = new MimeMessage();
                    email.From.Add(MailboxAddress.Parse(_config["EmailSender:SenderEmail"]));
                    email.To.Add(MailboxAddress.Parse(newUser.Email));
                    email.Subject = "Account Created at TimeSheet Management System";

                    string wwwrootPath = _webHostEnvironment.WebRootPath;
                    string templateFilePath = Path.Combine(wwwrootPath, "EmailTemplates/Registration_Email_template.html");
                    string htmlTemplate = await System.IO.File.ReadAllTextAsync(templateFilePath);
                    htmlTemplate = htmlTemplate.Replace("#FullName#", newUser.FirstName + " " + newUser.LastName).Replace("#Password#", newUser.Password).Replace("#Username#", newUser.UserName).Replace("#Email#", newUser.Email);

                    email.Body = new TextPart(TextFormat.Html) { Text = htmlTemplate };
                    MailData mailData = new MailData
                    {
                        EmailBody = htmlTemplate,
                        EmailSubject = "Account Created at TimeSheet Management System",
                        EmailToId = newUser.Email,
                        EmailToName = newUser.FirstName + " " + newUser.LastName
                    };

                    var sendMail = _mailService.SendMail(mailData);
                    if (!sendMail)
                    {
                        Message += ", Email Not Send";
                    }
                    else
                    {
                        Message += ", Email Send";
                    }
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
        #endregion

        #region GetUserLoginInformation

        [HttpGet("GetUserLoginInformation")]
        public async Task<ResponseDTO<PostUserDTO>> GetUserLoginInformation(string userId)
        {
            ResponseDTO<PostUserDTO> response = new ResponseDTO<PostUserDTO>();
            int StatusCode = 0;
            bool isSuccess = false;
            PostUserDTO Response = null;
            string Message = "";
            string ExceptionMessage = "";

            try
            {
                ApplicationUser user = await _IUserService.GetById(userId);
                if (user != null)
                {
                    var userDTO = _mapper.Map<PostUserDTO>(user);
                    Response = userDTO;
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Accound Details fetched successfully";
                    _logger.LogInformation("Accound Details fetched successfully");
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
        #endregion

        #region ChangePassword

        [HttpPost("ChangePassword")]
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
                var user = await _userResolverService.GetCurrentUser();
                if (user.Email == vmChangePassword.Email)
                {
                    var result = await _IUserService.ChangePasswordAsync(vmChangePassword.Email, vmChangePassword.CurrentPassword, vmChangePassword.NewPassword, vmChangePassword.ConfirmPassword);
                if (!result)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Email Does not Exists -Current Password Does not match";
                }

                else
                {
                    isSuccess = true;
                    StatusCode = 200;
                    Message = "New Password Created";
                }
                }
                else
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "User Not Found!";
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
        #endregion

        #region Update Profile
        [HttpPut("UpdateUser")]
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
                var user = await _userResolverService.GetCurrentUser();
                var userId = user.Id.ToString();
                if (user != null)
                {
                    ApplicationUser existingUser = await _IUserService.GetById(userId);
                    if(existingUser != null) {
                        existingUser.UserName = newUser.UserName;
                        existingUser.FirstName = newUser.FirstName;
                        existingUser.LastName = newUser.LastName;
                        existingUser.PostalCode = newUser.PostalCode;
                        existingUser.MobileNo = newUser.MobileNo;
                        existingUser.Address1 = newUser.Address1;
                        existingUser.Address2 = newUser.Address2;
                        existingUser.City = newUser.City;
                        existingUser.State = newUser.State;
                        existingUser.Country = newUser.Country;
                        existingUser.UpdatedBy = user.Id;
                        existingUser.UpdatedOn = DateTime.UtcNow;
                        existingUser.SecurityStamp = user.SecurityStamp;
                    }
                    IdentityResult result = await _IUserService.UpdateUser(existingUser);
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
        #endregion

        #region GetAllUser
        [HttpGet("GetAllUser")]
        public async Task<ResponseDTO<IEnumerable<UserResponseDTO>>> GetAllUser()
        {
            ResponseDTO<IEnumerable<UserResponseDTO>> response = new ResponseDTO<IEnumerable<UserResponseDTO>>();
            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable<UserResponseDTO> Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
               
                var getAllUser = await _IUserService.GetAllUsers();
                if (getAllUser == null)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Invalid data";
                }
                else
                {
                    IEnumerable<UserResponseDTO> allUser = _mapper.Map<IEnumerable<UserResponseDTO>>(getAllUser);
                    foreach (var item in getAllUser)
                    {
                        var UserRoleFetch = await _IUserService.GetUserRole(item);
                        var thisUserRole = allUser.Single(x => x.Id == item.Id);
                        thisUserRole.UserRole = UserRoleFetch.First().ToString();
                          
                    }


                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Successfully Get all user.";
                    Response = allUser;
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
        #endregion

        #region GetAllManagers
        [HttpGet("GetAllManagers")]
        public async Task<ResponseDTO<IEnumerable<UserResponseDTO>>> GetAllManagers()
        {
            ResponseDTO<IEnumerable<UserResponseDTO>> response = new ResponseDTO<IEnumerable<UserResponseDTO>>();
            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable<UserResponseDTO> Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var getAllUser = await _IUserService.GetAllUsers();
                if (getAllUser == null)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Invalid data";
                }
                else
                {

                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Successfully Get all user.";
                    IEnumerable<UserResponseDTO> allUser = _mapper.Map<IEnumerable<UserResponseDTO>>(getAllUser);
                    Response = allUser;
                    var Roles = "Managers";
                    var userRole = await _IUserService.GetUsersByRole(Roles);

                    if (userRole == null)
                    {
                        isSuccess = false;
                        StatusCode = 400;
                        Message = "Invalid data";
                    }
                    else
                    {
                        StatusCode = 200;
                        isSuccess = true;
                        IEnumerable<UserResponseDTO> usersRole = _mapper.Map<IEnumerable<UserResponseDTO>>(userRole);
                        Message = "Successfully Get all user.";
                        Response = usersRole;
                    }
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
        #endregion

        #region ResetPassword
        [AllowAnonymous]
        [HttpPost("ResetPassword")]
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
                var user = await _userManager.FindByEmailAsync(resetPass.Email);
                if (user == null)
                {
                    response.StatusCode = 400;
                    response.IsSuccess = false;
                    response.Message = "Invalid Email Address, Looks like You have'nt created Account. ";
                    return response;
                }
                else
                {
                    var result = await _IUserService.ResetPasswordAsync(user, resetPass.Code, resetPass.NewPassword);
                    isSuccess = false;
                    StatusCode = 200;
                    Message = "Password Changed completed successfully";
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
        #endregion

        #region ForgetPassword
        [HttpPost("ForgetPassword")]
        [AllowAnonymous]
        public async Task<ResponseDTO<string>> ForgetPassword(string Email)
        {
            ResponseDTO<string> response = new ResponseDTO<string>();
            int StatusCode = 0;
            bool isSuccess = false;
            LoginResponseDTO Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {           
                var user = await _userManager.FindByEmailAsync(Email);
                //var user = await _IUserService.GetUserByEmail(Email);
                if (user == null)
                {
                    response.StatusCode = 400;
                    response.IsSuccess = false;
                    response.Message = "Invalid Email Address, Looks like You have'nt created Account. ";
                    return response;
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                //var newPassword = UtilityHelper.GenerateRandomPassword(8);
                
                if (Email!=null)
                {
                    // send New Password To Registered Email
                    var email = new MimeMessage();
                    email.From.Add(MailboxAddress.Parse("rahulmakwana.convergesol@gmail.com"));
                    email.To.Add(MailboxAddress.Parse(Email));
                    email.Subject = "Reset Password at TimeSheet Management System";

                    string wwwrootPath = _webHostEnvironment.WebRootPath;
                    string templateFilePath = Path.Combine(wwwrootPath, "EmailTemplates/Reset_Password_Email_template.html");
                    string htmlTemplate = await System.IO.File.ReadAllTextAsync(templateFilePath);
                    htmlTemplate = htmlTemplate.Replace("#FullName#", user.FirstName + " " + user.LastName).Replace("#Code#", code);

                    email.Body = new TextPart(TextFormat.Html) { Text = htmlTemplate };

                    MailData mailData = new MailData
                    {
                        EmailBody = htmlTemplate,
                        EmailSubject = "Reset Password at TimeSheet Management System",
                        EmailToId = Email,
                        EmailToName = Email
                    };
                    var sendMail = _mailService.SendMail(mailData);
                    if (!sendMail)
                    {
                        isSuccess = false;
                        StatusCode =400;
                        Message = "Email Not Send";
                    }
                    else
                    {
                        isSuccess = true;
                        StatusCode = 200;
                        Message = " Email Send";
                    }
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
        #endregion

        #region UserSignOut

        [HttpGet("SignOut")]
        public async Task<ResponseDTO<string>> SignOut()
        {
            ResponseDTO<string> response = new ResponseDTO<string>();
            int StatusCode = 0;
            bool isSuccess = false;
            LoginResponseDTO Response = null;
            string Message = "";
            string ExceptionMessage = "";
            var user = await _userResolverService.GetCurrentUser();
            if (user != null)
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
        #endregion

    }
}