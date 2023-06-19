using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Infrastructure.Context;
using TMS.Infrastructure.Interfaces;

namespace TMS.Infrastructure.Repository
{
	public class TimesheetApprovalsRepository : ITimesheetApprovalsRepository
	{
		private readonly IUnitOfWork _unitOfWork;
		public TimesheetApprovalsRepository(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<int> CreateTimesheetApproval(TimeSheetApprovals timesheetApproval)
		{
			_unitOfWork.Context.TimeSheetApprovals.Add(timesheetApproval);
			await _unitOfWork.Context.SaveChangesAsync();
			return timesheetApproval.Id;
		}

		public async Task<int> DeleteTimesheetApproval(int timesheetApprovalId)
		{
			TimeSheetApprovals timeSheetApprovals = await _unitOfWork.Context.TimeSheetApprovals.FindAsync(timesheetApprovalId);
			_unitOfWork.Context.TimeSheetApprovals.Remove(timeSheetApprovals);
			await _unitOfWork.Context.SaveChangesAsync();
			return timesheetApprovalId;
		}

		public async Task<TimeSheetApprovals> GetTimesheetApproval(int timesheetApprovalId)
		{
			var data = await _unitOfWork.Context.TimeSheetApprovals.FindAsync(timesheetApprovalId);
			await _unitOfWork.Context.SaveChangesAsync();
			return data;
		}

		public async Task<IEnumerable<TimeSheetApprovals>> GetTimesheetApprovals()
		{
			var data = await _unitOfWork.Context.TimeSheetApprovals.ToListAsync();

			//if (fromDate != null)
			//{
			//	query = query.Where(x => x.CreatedOn >= fromDate);
			//}

			//if (toDate != null)
			//{
			//	query = query.Where(x => x.LastModifiedOn <= toDate);
			//}
			await _unitOfWork.Context.SaveChangesAsync();
			return data;
		}

		public async Task<int> UpdateTimesheetApproval(TimeSheetApprovals timesheetApproval)
		{
			int entry = 0;
			TimeSheetApprovals olddata = await _unitOfWork.Context.TimeSheetApprovals.FindAsync(timesheetApproval.Id);
			if (olddata != null)
			{
				olddata.TimeSheetMasterId = timesheetApproval.TimeSheetMasterId;
				olddata.ApprovalStatus = timesheetApproval.ApprovalStatus;
				olddata.ApprovalStatusBy = timesheetApproval.ApprovalStatusBy;
				olddata.CreatedOn = timesheetApproval.CreatedOn;
				olddata.CreatedBy = timesheetApproval.CreatedBy;
				olddata.LastModifiedOn = timesheetApproval.LastModifiedOn;
				olddata.LastModifiedBy = timesheetApproval.LastModifiedBy;
				entry = await _unitOfWork.Context.SaveChangesAsync();
			}
			return entry;
		}
	}
}
