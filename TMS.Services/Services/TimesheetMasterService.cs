using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Infrastructure.Interfaces;
using TMS.Services.Interfaces;

namespace TMS.Services.Services
{
    public class TimesheetMasterService : ITimesheetMasterService
    {
        private readonly ITimesheetMasterRepository _timesheetMasterRepository;

        public TimesheetMasterService(ITimesheetMasterRepository timesheetMasterRepository)
        {
            _timesheetMasterRepository = timesheetMasterRepository;
        }
        public async Task<int> CreateTimesheetMaster(TimeSheetMaster timesheetMaster)
        {
            return await _timesheetMasterRepository.CreateTimesheetMaster(timesheetMaster);
        }

        public async Task<int> DeleteTimesheetMaster(int id)
        {
            return await _timesheetMasterRepository.DeleteTimesheetMaster(id);
        }

        public async Task<TimeSheetMaster> GetTimesheetMaster(int id)
        {
            return await _timesheetMasterRepository.GetTimesheetMaster(id);
        }

        public async Task<IEnumerable<TimeSheetMaster>> GetTimesheetMastersAll()
        {
            return await _timesheetMasterRepository.GetTimesheetMastersAll();
        }

        public Task<int> UpdateTimesheetMaster(TimeSheetMaster timesheetMaster)
        {
            return _timesheetMasterRepository.UpdateTimesheetMaster(timesheetMaster);
        }
    }
}
