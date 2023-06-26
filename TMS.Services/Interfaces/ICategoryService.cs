using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;

namespace TMS.Infrastructure.Interfaces
{
    public interface ICategoryService
    {

        Task<IEnumerable<Category>> GetAll();
        Task<Category> GetById(int id);
        Task<int> Add(Category category);
        Task<int> Update(Category category);
        Task<int> Delete(int id);
    }
}
