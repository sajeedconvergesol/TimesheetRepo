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
            _unitOfWork.Context.Projects.Add(project);
            await _unitOfWork.Context.SaveChangesAsync();
            return project.Id;
        }

        public async Task<int> Delete(int id)
        {
            {
                Project project = await _unitOfWork.Context.Projects.FindAsync(id);
                _unitOfWork.Context.Projects.Remove(project);
                await _unitOfWork.Context.SaveChangesAsync();
                return id;
            }
        }
        public async Task<IEnumerable<Project>> GetAll()
        {
                var data = await _unitOfWork.Context.Projects.ToListAsync();
                await _unitOfWork.Context.SaveChangesAsync();
                return data;
            }

        public async Task<Project> GetById(int id)
        {

                var data = await _unitOfWork.Context.Projects.FindAsync();
                await _unitOfWork.Context.SaveChangesAsync();
                return data;
            }

        public async Task<int> Update(Project project)
        {

                int entry = 0;
                Project olddata = await _unitOfWork.Context.Projects.FindAsync(project.Id);
                if (olddata != null)
                {
                olddata.ProjectName = project.ProjectName;
                olddata.StartDate = project.StartDate;
                olddata.EndDate = project.EndDate;
                entry = await _unitOfWork.Context.SaveChangesAsync();
                }
                return entry;
            }
    }
}
