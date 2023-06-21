using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Infrastructure.Interfaces;

namespace TMS.Infrastructure.Repository
{
    public class ProjectDocumentRepository : IProjectDetailsRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProjectDocumentRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Add(ProjectDocuments projectDocuments)
        {
            _unitOfWork.Context.ProjectDocuments.Add(projectDocuments);
            await _unitOfWork.Context.SaveChangesAsync();
            return projectDocuments.Id;
        }

        public async Task<int> Delete(int id)
        {
            ProjectDocuments projectDocument = await _unitOfWork.Context.ProjectDocuments.FindAsync(id);
            _unitOfWork.Context.ProjectDocuments.Remove(projectDocument);
            await _unitOfWork.Context.SaveChangesAsync();
            return id;
        }

        public async Task<IEnumerable<ProjectDocuments>> GetAll()
        {
            var data = await _unitOfWork.Context.ProjectDocuments.ToListAsync();
            await _unitOfWork.Context.SaveChangesAsync();
            return data;
        }

        public async Task<ProjectDocuments> GetById(int id)
        {
            var data = await _unitOfWork.Context.ProjectDocuments.FindAsync();
            await _unitOfWork.Context.SaveChangesAsync();
            return data;
        }

        public async Task<int> Update(ProjectDocuments projectDocuments)
        {
            int entry = 0;
            ProjectDocuments olddata = await _unitOfWork.Context.ProjectDocuments.FindAsync(projectDocuments.Id);
            if (olddata != null)
            {
                olddata.ProjectId = projectDocuments.ProjectId;
                olddata.CreatedOn = projectDocuments.CreatedOn;
                olddata.ModifiedOn = projectDocuments.ModifiedOn;                
                olddata.CreatedBy = projectDocuments.CreatedBy;
                olddata.DocumentName = projectDocuments.DocumentName;
                olddata.LastModifiedBy = projectDocuments.LastModifiedBy;
                entry = await _unitOfWork.Context.SaveChangesAsync();
            }
            return entry;
        }
        public async Task<List<ProjectDocuments>> GetByProjectId(int projectId)
        {
            return _unitOfWork.Context.ProjectDocuments.Where(z => z.ProjectId == projectId).ToList();
        }
    }
}
