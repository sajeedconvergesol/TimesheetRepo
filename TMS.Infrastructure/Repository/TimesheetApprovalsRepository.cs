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
			await _unitOfWork.Context.Set<TimeSheetApprovals>().AddAsync(timesheetApproval);
            _unitOfWork.Commit();
			return timesheetApproval.Id;
		}

		public async Task<bool> DeleteTimesheetApproval(int timesheetApprovalId)
		{
			bool isDeleted;
			try
			{
                TimeSheetApprovals timeSheetApprovals = await _unitOfWork.Context.TimeSheetApprovals.Where(x => x.Id == timesheetApprovalId).FirstOrDefaultAsync();
                _unitOfWork.Context.TimeSheetApprovals.Remove(timeSheetApprovals);
                _unitOfWork.Commit();
				isDeleted = true;
            }
			catch
			{
				isDeleted = false;
			}			
            return isDeleted;
		}

		public async Task<TimeSheetApprovals> GetTimesheetApproval(int timesheetApprovalId)
		{
			var data = _unitOfWork.Context.TimeSheetApprovals.Where(x => x.Id == timesheetApprovalId).FirstOrDefaultAsync();
            return await data;
		}

		public async Task<IEnumerable<TimeSheetApprovals>> GetTimesheetApprovals()
		{
			var data = _unitOfWork.Context.TimeSheetApprovals;
			return await data.ToListAsync();
		}

		public int UpdateTimesheetApproval(TimeSheetApprovals timesheetApproval)
		{
            _unitOfWork.Context.Entry(timesheetApproval).State = EntityState.Modified;
            _unitOfWork.Context.TimeSheetApprovals.Update(timesheetApproval);
            _unitOfWork.Commit();
            return timesheetApproval.Id;
        }
		public async Task<TimeSheetApprovals> GetTimeApprovalByTimeSheetId(int timeSheetId)
		{
			var timeSheetApproval = _unitOfWork.Context.TimeSheetApprovals.Where(x => x.TimeSheetMasterId == timeSheetId).FirstOrDefaultAsync();
			return await timeSheetApproval;
		}
    }
}
