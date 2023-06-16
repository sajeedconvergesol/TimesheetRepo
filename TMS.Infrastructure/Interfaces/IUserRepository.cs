using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;

namespace TMS.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsers(int offset, int pageSize);
        Task<IEnumerable<ApplicationUser>> GetAllUsers();
        Task<IEnumerable<ApplicationUser>> GetActiveUsers(int offset, int pageSize);
        Task<IdentityResult> Create(ApplicationUser user, string password, string role);
        Task<bool> ConfirmEmail(string userId, string code);
        Task<bool> ResetPassword(ApplicationUser obUser, string code, string password);
        Task<bool> ChangePassword(string email, string oldPassword, string newPassword, string confirmPassword);
        Task<ApplicationUser> GetUserByEmail(string userEmail);
        Task<IdentityResult> UpdateUser(ApplicationUser obUser);
        Task<IList<string>> GetUserRole(ApplicationUser obUser);
        Task<bool> Delete(string userId);
        Task<ApplicationUser> GetById(string id);
        Task<ApplicationUser> GetByUserName(string userName);
        Task<bool> IsExists(string email);
    }
}
