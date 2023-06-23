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
    public class TaskAssignmentService : ITaskAssignmentService
    {
        private readonly ITaskAssignmentRepository _taskAssignmentRepository;

        public TaskAssignmentService(ITaskAssignmentRepository taskAssignmentRepository)
        {
            _taskAssignmentRepository = taskAssignmentRepository;
        }

        public async Task<int> Add(TaskAssignment taskAssignment)
        {
            return await _taskAssignmentRepository.Add(taskAssignment);
        }

        public async Task<bool> Delete(int id)
        {
            return await _taskAssignmentRepository.Delete(id);
        }

        public async Task<IEnumerable<TaskAssignment>> GetAll()
        {
            return await _taskAssignmentRepository.GetAll();
        }

        public async Task<TaskAssignment> GetById(int id)
        {
            return await _taskAssignmentRepository.GetById(id);
        }

        public int Update(TaskAssignment taskAssignment)
        {
            return _taskAssignmentRepository.Update(taskAssignment);
        }
    }
}
