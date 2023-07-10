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
    public class ProjectRepository : IProjectRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Add(Project project)
        {
            await _unitOfWork.Context.Set<Project>().AddAsync(project);
            _unitOfWork.Commit();
            return project.Id;
        }

        public async Task<bool> Delete(int id)
        {
            bool isDeleted;
            try
            {
                Project project = await _unitOfWork.Context.Projects.Where(x => x.Id == id).FirstOrDefaultAsync();
                _unitOfWork.Context.Projects.Remove(project);
                _unitOfWork.Commit();
                isDeleted = true;
            }
            catch
            {
                isDeleted = false;                
            }
            
            return isDeleted;
        }
        public async Task<IEnumerable<Project>> GetAll()
        {
            var data = _unitOfWork.Context.Projects;
            return await data.ToListAsync();
        }

        public async Task<Project> GetById(int id)
        {
            var data = _unitOfWork.Context.Projects.Where(x => x.Id == id).FirstOrDefaultAsync();
            return await data;
        }

        public int Update(Project project)
        {
            _unitOfWork.Context.Entry(project).State = EntityState.Modified;
            _unitOfWork.Context.Projects.Update(project);
            _unitOfWork.Commit();
            return project.Id;
        }
    }
}
