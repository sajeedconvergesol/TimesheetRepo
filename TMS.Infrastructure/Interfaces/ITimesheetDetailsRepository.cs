using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;

namespace TMS.Infrastructure.Interfaces
{
	public interface ITimesheetDetailsRepository
	{
		Task<IEnumerable<TimeSheetDetails>> GetAll();
		Task<TimeSheetDetails> GetById(int id);
		Task<int> Add(TimeSheetDetails timesheetDetail);
		Task<int> Update(TimeSheetDetails timesheetDetail);
		Task<int> Delete(int id);
        Task<IEnumerable<TimeSheetDetails>> GetByTimeSheetMasterId(int id);
    }
}
