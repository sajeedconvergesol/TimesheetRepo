using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Infrastructure.InfrastructureHelper;
using TMS.Infrastructure.Interfaces;

namespace TMS.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {   
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }


        public async Task<IEnumerable<ApplicationUser>> GetAllUsers(int offset, int pageSize)
        {
            var result = _unitOfWork.Context.Users;
            return await PaginatedList<ApplicationUser>.CreateAsync(result.AsNoTracking(), offset, pageSize);
        }
        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            var result = _unitOfWork.Context.Users;
            return await result.ToListAsync();
        }
        public async Task<IEnumerable<ApplicationUser>> GetActiveUsers(int offset, int pageSize)
        {
            var result = _unitOfWork.Context.Users.Where(x => x.IsActive);
            return await PaginatedList<ApplicationUser>.CreateAsync(result.AsNoTracking(), offset, pageSize);
        }
        public async Task<IList<string>> GetUserRole(ApplicationUser obUser)
        {
            var role = await _userManager.GetRolesAsync(obUser);
            return role;
        }
        public async Task<IdentityResult> Create(ApplicationUser obUser, string password, string role)
        {
            var user = new ApplicationUser
            {
                UserName = obUser.UserName,
                Email = obUser.Email,
                FirstName = obUser.FirstName,
                LastName = obUser.LastName,
                NormalizedUserName = obUser.NormalizedUserName,
                NormalizedEmail = obUser.NormalizedEmail,
                EmailConfirmed = obUser.EmailConfirmed,
                PhoneNumber = obUser.PhoneNumber,
                PhoneNumberConfirmed = obUser.PhoneNumberConfirmed,
                MobileNo = obUser.MobileNo,
                TwoFactorEnabled = obUser.TwoFactorEnabled,
                LockoutEnd = obUser.LockoutEnd,
                LockoutEnabled = obUser.LockoutEnabled,
                AccessFailedCount = obUser.AccessFailedCount,
                IsActive = obUser.IsActive,
                Address1 = obUser.Address1,
                Address2 = obUser.Address2,
                City = obUser.City,
                State = obUser.State,
                UpdatedBy = obUser.UpdatedBy,
                UpdatedOn = obUser.UpdatedOn,
                ManagerId = obUser.ManagerId,
                DateOfBirth = obUser.DateOfBirth,
                Country = obUser.Country,
                PostalCode = obUser.PostalCode,
                Gender = obUser.Gender,
                CreatedOn = DateTime.Now,
                CreatedBy = obUser.CreatedBy
            };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);
            }
            return result;
        }
        public async Task<bool> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return false;
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return result.Succeeded ? result.Succeeded : false;
        }
        public async Task<bool> ResetPassword(ApplicationUser obUser, string code, string password)
        {
            var user = await _userManager.FindByEmailAsync(obUser.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return false;
            }
            var result = await _userManager.ResetPasswordAsync(user, code, password);
            if (result.Succeeded)
            {
                return result.Succeeded;
            }
            return false;
        }
        public async Task<bool> ChangePassword(string email, string oldPassword, string newPassword, string confirmPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            //if (newPassword != confirmPassword)
            //{
            //	throw new AppException("new password and confirm password does not match.");
            //}
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
                return result.Succeeded;
            }
            else
            {
                // Don't reveal that the user does not exist
                return false;
            }
        }
        public async Task<ApplicationUser> GetUserByEmail(string userEmail)
        {
            try { 
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user != null)
                return user;
            else
                return null;
        }
            catch(Exception e)
            {
                
            }
            return null;
        }
        public async Task<ApplicationUser> GetByUserName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
                return user;
            else
                return null;
        }
        public async Task<IdentityResult> UpdateUser(ApplicationUser obUser)
        {
            obUser.UpdatedOn = DateTime.Now;

            var result = await _userManager.UpdateAsync(obUser);
            return result;
        }

        public async Task<bool> Delete(string userId)
        {
            if (userId == null)
            {
                return false;
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded ? result.Succeeded : false;
        }
        public async Task<ApplicationUser> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return null;
            else
                return user;
        }
        public async Task<bool> IsExists(string email)
        {
            bool IsExists = false;
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                IsExists = true;
            }
            return IsExists;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersByRole(string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);
            return users;
        }
    }
}
