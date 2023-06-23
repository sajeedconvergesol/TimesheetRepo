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
    public class TimesheetApprovalsService : ITimesheetApprovalsService
    {
        private readonly ITimesheetApprovalsRepository _timesheetApprovalsRepository;

        public TimesheetApprovalsService(ITimesheetApprovalsRepository timesheetApprovalsRepository)
        {
            _timesheetApprovalsRepository = timesheetApprovalsRepository;
        }

        public Task<int> CreateTimesheetApproval(TimeSheetApprovals timesheetApproval)
        {
            return _timesheetApprovalsRepository.CreateTimesheetApproval(timesheetApproval);
        }

        public async Task<bool> DeleteTimesheetApproval(int timesheetApprovalId)
        {
            return await _timesheetApprovalsRepository.DeleteTimesheetApproval(timesheetApprovalId);
        }

        public async Task<TimeSheetApprovals> GetTimesheetApproval(int timesheetApprovalId)
        {
            return await _timesheetApprovalsRepository.GetTimesheetApproval(timesheetApprovalId);
        }

        public async Task<IEnumerable<TimeSheetApprovals>> GetTimesheetApprovals()
        {
            return await _timesheetApprovalsRepository.GetTimesheetApprovals();
        }

        public int UpdateTimesheetApproval(TimeSheetApprovals timesheetApproval)
        {
            return _timesheetApprovalsRepository.UpdateTimesheetApproval(timesheetApproval);
        }
        public async Task<TimeSheetApprovals> GetTimeApprovalByTimeSheetId(int timeSheetId)
        {
            return await _timesheetApprovalsRepository.GetTimeApprovalByTimeSheetId(timeSheetId);
        }
    }
}
