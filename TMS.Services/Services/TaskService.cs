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

        public Task<int> Add(Tasks task)
        {
            return _taskRepository.Add(task);
        }

        public Task<int> Delete(int id)
        {
            return _taskRepository.Delete(id);
        }

        public Task<IEnumerable<Tasks>> GetAll()
        {
            return _taskRepository.GetAll();
        }

        public Task<Tasks> GetById(int id)
        {
            return _taskRepository.GetById(id);
        }

        public Task<int> Update(Tasks task)
        {
            return _taskRepository.Update(task);
        }
    }
}
