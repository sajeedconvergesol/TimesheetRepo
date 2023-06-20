using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Infrastructure.Interfaces;

namespace TMS.Services.Services
{
    public class ProjectDocumentService : IProjectDocumentService
    {
        private readonly IProjectDetailsRepository _projectDetailsRepository;

        public ProjectDocumentService(IProjectDetailsRepository projectDetailsRepository)
        {
            _projectDetailsRepository = projectDetailsRepository;
        }

        public Task<int> Add(ProjectDocuments projectDocuments)
        {
            return _projectDetailsRepository.Add(projectDocuments);
        }

        public Task<int> Delete(int id)
        {
            return _projectDetailsRepository.Delete(id);

        }

        public Task<IEnumerable<ProjectDocuments>> GetAll()
        {
            return _projectDetailsRepository.GetAll();
        }

        public Task<ProjectDocuments> GetById(int id)
        {
            return _projectDetailsRepository.GetById(id);
        }

        public Task<int> Update(ProjectDocuments projectDocuments)
        {
            return _projectDetailsRepository.Update(projectDocuments);
        }
        public Task<List<ProjectDocuments>> GetByProjectId (int projectId)
        {
            return _projectDetailsRepository.GetByProjectId(projectId);
        }
    }
}
