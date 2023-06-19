using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;

namespace TMS.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(int offset, int pageSize);
        Task<IEnumerable<ApplicationUser>> GetAllUsers();
        Task<IEnumerable<ApplicationUser>> GetActiveUsersAsync(int offset, int pageSize);
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password, string role);
        Task<bool> ConfirmEmailAsync(string userId, string code);
        Task<bool> ResetPasswordAsync(ApplicationUser obUser, string code, string password);
        Task<bool> ChangePasswordAsync(string email, string oldPassword, string newPassword, string confirmPassword);
        Task<ApplicationUser> GetUserByEmail(string userEmail);
        Task<IdentityResult> UpdateUser(ApplicationUser obUser);
        Task<IList<string>> GetUserRole(ApplicationUser obUser);
        Task<bool> DeleteAsync(string userId);
        Task<ApplicationUser> GetById(string id);
        Task<ApplicationUser> GetByUserName(string userName);
        Task<bool> IsExists(string email);
    }
}
