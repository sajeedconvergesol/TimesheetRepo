using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Infrastructure.Interfaces;

namespace TMS.Infrastructure.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ApplicationRole>> GetAllRoles()
        {
            var result = _unitOfWork.Context.Roles;
            return await result.ToListAsync();
        }
        public ApplicationRole GetByRoleName(string roleName)
        {
            var role = _unitOfWork.Context.Roles.Where(x => x.Name == roleName).FirstOrDefault();
            return role;
        }
    }
}
