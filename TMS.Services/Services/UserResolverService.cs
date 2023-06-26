using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Services.Interfaces;

namespace TMS.Services.Services
{
    public class UserResolverService : IUserResolverService
    {
        private readonly IHttpContextAccessor _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserResolverService(IHttpContextAccessor context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(_context.HttpContext.User);
        }
    }
}
