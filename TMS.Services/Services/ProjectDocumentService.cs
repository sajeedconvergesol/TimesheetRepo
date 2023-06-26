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
        private readonly IProjectDocumentRepository _projectDetailsRepository;

        public ProjectDocumentService(IProjectDocumentRepository projectDetailsRepository)
        {
            _projectDetailsRepository = projectDetailsRepository;
        }

        public async Task<int> Add(ProjectDocuments projectDocuments)
        {
            return await _projectDetailsRepository.Add(projectDocuments);
        }

        public async Task<bool> Delete(int id)
        {
            return await _projectDetailsRepository.Delete(id);

        }

        public async Task<IEnumerable<ProjectDocuments>> GetAll()
        {
            return await _projectDetailsRepository.GetAll();
        }

        public async Task<ProjectDocuments> GetById(int id)
        {
            return await _projectDetailsRepository.GetById(id);
        }

        public async Task<int> Update(ProjectDocuments projectDocuments)
        {
            return await _projectDetailsRepository.Update(projectDocuments);
        }
        public async Task<IEnumerable<ProjectDocuments>> GetByProjectId(int projectId)
        {
            return await _projectDetailsRepository.GetByProjectId(projectId);
        }
    }
}
