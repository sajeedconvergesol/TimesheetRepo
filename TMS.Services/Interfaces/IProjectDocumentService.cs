using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;

namespace TMS.Infrastructure.Interfaces
{
    public interface IProjectDocumentService
    {
        Task<IEnumerable<ProjectDocuments>> GetAll();
        Task<ProjectDocuments> GetById(int id);
        Task<int> Add(ProjectDocuments projectDocuments);
        Task<int> Update(ProjectDocuments projectDocuments);
        Task<int> Delete(int id);
        Task<List<ProjectDocuments>> GetByProjectId(int projectId);
    }
}
