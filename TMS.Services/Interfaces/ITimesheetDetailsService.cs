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
        Task<bool> Add(List<TimeSheetDetails> timesheetDetailList);
        int Update(TimeSheetDetails timesheetDetail);
        Task<bool> Delete(int id);
        bool DeleteByTimeSheetId(int timesheetId);
        Task<IEnumerable<TimeSheetDetails>> GetByTimeSheetMasterId(int id);
    }
}
