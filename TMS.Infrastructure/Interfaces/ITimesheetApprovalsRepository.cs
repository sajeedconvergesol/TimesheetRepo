using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;

namespace TMS.Infrastructure.Interfaces
{
	public interface ITimesheetApprovalsRepository
	{
		Task<TimeSheetApprovals> GetTimesheetApproval(int timesheetApprovalId);
		Task<IEnumerable<TimeSheetApprovals>> GetTimesheetApprovals();
		Task<int> CreateTimesheetApproval(TimeSheetApprovals timesheetApproval);
		Task<int> UpdateTimesheetApproval(TimeSheetApprovals timesheetApproval);
		Task<int> DeleteTimesheetApproval(int timesheetApprovalId);

	}
}
