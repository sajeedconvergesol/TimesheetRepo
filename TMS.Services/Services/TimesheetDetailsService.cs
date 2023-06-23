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
    public class TimesheetDetailsService : ITimesheetDetailsService
    {
        private readonly ITimesheetDetailsRepository _timesheetDetailsRepository;

        public TimesheetDetailsService(ITimesheetDetailsRepository timesheetDetailsRepository)
        {
            _timesheetDetailsRepository = timesheetDetailsRepository;
        }

        public async Task<bool> Add(List<TimeSheetDetails> timesheetDetailList)
        {
            return await _timesheetDetailsRepository.Add(timesheetDetailList);
        }

        public async Task<bool> Delete(int id)
        {
            return await _timesheetDetailsRepository.Delete(id);
        }

        public async Task<IEnumerable<TimeSheetDetails>> GetAll()
        {
            return await _timesheetDetailsRepository.GetAll();
        }

        public async Task<TimeSheetDetails> GetById(int id)
        {
            return await _timesheetDetailsRepository.GetById(id);
        }

        public int Update(TimeSheetDetails timesheetDetail)
        {
            return _timesheetDetailsRepository.Update(timesheetDetail);
        }
        public async Task<IEnumerable<TimeSheetDetails>> GetByTimeSheetMasterId(int id)
        {
            return await _timesheetDetailsRepository.GetByTimeSheetMasterId(id);
        }
        public bool DeleteByTimeSheetId(int timesheetId)
        {
            return _timesheetDetailsRepository.DeleteByTimeSheetId(timesheetId);
        }
    }
}
