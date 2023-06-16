using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();
        IdentityRole GetByRoleName(string roleName);
    }
}
