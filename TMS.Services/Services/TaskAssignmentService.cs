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

        public Task<int> Add(TaskAssignment taskAssignment)
        {
            return _taskAssignmentRepository.Add(taskAssignment);
        }

        public Task<int> Delete(int id)
        {
            return _taskAssignmentRepository.Delete(id);
        }

        public Task<IEnumerable<TaskAssignment>> GetAll()
        {
            return _taskAssignmentRepository.GetAll();
        }

        public Task<TaskAssignment> GetById(int id)
        {
            return _taskAssignmentRepository.GetById(id);
        }

        public Task<int> Update(TaskAssignment taskAssignment)
        {
            return _taskAssignmentRepository.Update(taskAssignment);
        }
    }
}
