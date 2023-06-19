using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;

namespace TMS.Services.Interfaces
{
    public interface ITimesheetDetailsService
    {
        Task<IEnumerable<TimeSheetDetails>> GetAll();
        Task<TimeSheetDetails> GetById(int id);
        Task<int> Add(TimeSheetDetails timesheetDetail);
        Task<int> Update(TimeSheetDetails timesheetDetail);
        Task<int> Delete(int id);
    }
}
