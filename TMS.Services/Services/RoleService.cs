using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Infrastructure.Interfaces;
using TMS.Services.Interfaces;

namespace TMS.Services.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _iRoleRepository;
        public RoleService(IRoleRepository iRoleRepository)
        {
            _iRoleRepository = iRoleRepository;
        }

        public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
        {
            return await _iRoleRepository.GetAllRoles();
        }

        public IdentityRole GetByRoleName(string roleName)
        {
            return _iRoleRepository.GetByRoleName(roleName);
        }
    }
}
