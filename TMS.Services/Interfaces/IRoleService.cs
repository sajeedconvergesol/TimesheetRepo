using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;

namespace TMS.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<ApplicationRole>> GetAllRolesAsync();
        ApplicationRole GetByRoleName(string roleName);
    }
}
