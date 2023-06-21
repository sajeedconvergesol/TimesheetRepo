using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;

namespace TMS.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAll();
        Task<Project> GetById(int id);
        Task<int> Add(Project project);
        Task<int> Update(Project project);
        Task<int> Delete(int id);
    }
}
