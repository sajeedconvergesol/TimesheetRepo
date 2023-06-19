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

        public Task<int> Add(TimeSheetDetails timesheetDetail)
        {
            return _timesheetDetailsRepository.Add(timesheetDetail);
        }

        public Task<int> Delete(int id)
        {
            return _timesheetDetailsRepository.Delete(id);
        }

        public Task<IEnumerable<TimeSheetDetails>> GetAll()
        {
            return _timesheetDetailsRepository.GetAll();
        }

        public Task<TimeSheetDetails> GetById(int id)
        {
            return _timesheetDetailsRepository.GetById(id);
        }

        public Task<int> Update(TimeSheetDetails timesheetDetail)
        {
            return _timesheetDetailsRepository.Update(timesheetDetail);
        }
    }
}
