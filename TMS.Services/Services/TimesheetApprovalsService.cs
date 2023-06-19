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

        public Task<int> DeleteTimesheetApproval(int timesheetApprovalId)
        {
            return _timesheetApprovalsRepository.DeleteTimesheetApproval(timesheetApprovalId);
        }

        public Task<TimeSheetApprovals> GetTimesheetApproval(int timesheetApprovalId)
        {
            return _timesheetApprovalsRepository.GetTimesheetApproval(timesheetApprovalId);
        }

        public Task<IEnumerable<TimeSheetApprovals>> GetTimesheetApprovals()
        {
            return _timesheetApprovalsRepository.GetTimesheetApprovals();
        }

        public Task<int> UpdateTimesheetApproval(TimeSheetApprovals timesheetApproval)
        {
            return _timesheetApprovalsRepository.UpdateTimesheetApproval(timesheetApproval);
        }
    }
}
