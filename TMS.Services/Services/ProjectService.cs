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
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repository;
        
        public ProjectService(IProjectRepository repository) {
            _repository = repository;
        }

        public Task<int> Add(Project project)
        {
            return _repository.Add(project);
        }

        public Task<int> Delete(int id)
        {
            return _repository.Delete(id);
        }

        public Task<IEnumerable<Project>> GetAll()
        {
            return _repository.GetAll();
        }

        public Task<Project> GetById(int id)
        {
            return _repository.GetById(id);
        }

        public Task<int> Update(Project project)
        {
            return _repository.Update(project);
        }
    }
}
