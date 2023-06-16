using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Infrastructure.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<IdentityRole>> GetAllRoles();
        IdentityRole GetByRoleName(string roleName);
    }
}
