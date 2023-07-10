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
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<int> Add(Tasks task)
        {
            return await _taskRepository.Add(task);
        }

        public async Task<bool> Delete(int id)
        {
            return await _taskRepository.Delete(id);
        }

        public async Task<IEnumerable<Tasks>> GetAll()
        {
            return await _taskRepository.GetAll();
        }

        public async Task<Tasks> GetById(int id)
        {
            return await _taskRepository.GetById(id);
        }

        public int Update(Tasks task)
        {
            return _taskRepository.Update(task);
        }
    }
}
