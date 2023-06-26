using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;

namespace TMS.Services.Interfaces
{
    public interface ITaskService
    {
        Task<Tasks> GetById(int id);
        Task<IEnumerable<Tasks>> GetAll();
        Task<int> Add(Tasks task);
        int Update(Tasks task);
        Task<bool> Delete(int id);
    }
}
