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
		int UpdateTimesheetApproval(TimeSheetApprovals timesheetApproval);
		Task<bool> DeleteTimesheetApproval(int timesheetApprovalId);
		Task<TimeSheetApprovals> GetTimeApprovalByTimeSheetId(int timeSheetId);
	}
}
