using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;

namespace TMS.Services.Interfaces
{
    public interface ITimesheetMasterService
    {
        Task<TimeSheetMaster> GetTimesheetMaster(int id);
        Task<IEnumerable<TimeSheetMaster>> GetTimesheetMastersAll();
        Task<int> CreateTimesheetMaster(TimeSheetMaster timesheetMaster);
        Task<int> UpdateTimesheetMaster(TimeSheetMaster timesheetMaster);
        Task<int> DeleteTimesheetMaster(int id);
    }
}
