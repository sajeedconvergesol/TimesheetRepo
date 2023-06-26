using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;

namespace TMS.Infrastructure.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<ApplicationRole>> GetAllRoles();
        ApplicationRole GetByRoleName(string roleName);
    }
}
