using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Infrastructure.Interfaces;
using TMS.Services.Interfaces;

namespace TMS.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _iUserRepository;
        public UserService(IUserRepository iUserRepository)
        {
            _iUserRepository = iUserRepository;
        }

        public async Task<bool> ChangePasswordAsync(string email, string oldPassword, string newPassword, string confirmPassword)
        => await _iUserRepository.ChangePassword(email, oldPassword, newPassword, confirmPassword);

        public async Task<bool> ConfirmEmailAsync(string userId, string code)
        => await _iUserRepository.ConfirmEmail(userId, code);

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password, string role)
        => await _iUserRepository.Create(user, password, role);

        public async Task<IEnumerable<ApplicationUser>> GetActiveUsersAsync(int offset, int pageSize)
        => await _iUserRepository.GetActiveUsers(offset, pageSize);

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(int offset, int pageSize)
        => await _iUserRepository.GetAllUsers(offset, pageSize);
        public async Task<IEnumerable<ApplicationUser>> GetAllUsers() => await _iUserRepository.GetAllUsers();

        public async Task<bool> ResetPasswordAsync(ApplicationUser obUser, string code, string password)
        => await _iUserRepository.ResetPassword(obUser, code, password);
        public async Task<ApplicationUser> GetUserByEmail(string userEmail)
        => await _iUserRepository.GetUserByEmail(userEmail);
        public async Task<IdentityResult> UpdateUser(ApplicationUser obUser)
        => await _iUserRepository.UpdateUser(obUser);
        public async Task<IList<string>> GetUserRole(ApplicationUser obUser) => await _iUserRepository.GetUserRole(obUser);
        public async Task<bool> DeleteAsync(string userId) => await _iUserRepository.Delete(userId);
        public async Task<ApplicationUser> GetById(string id) => await _iUserRepository.GetById(id);
        public async Task<ApplicationUser> GetByUserName(string userName) => await _iUserRepository.GetByUserName(userName);
        public async Task<bool> IsExists(string email)
            => await _iUserRepository.IsExists(email);
    }
}
