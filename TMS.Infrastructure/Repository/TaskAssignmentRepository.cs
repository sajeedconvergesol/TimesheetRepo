using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Infrastructure.Context;
using TMS.Infrastructure.Interfaces;

namespace TMS.Infrastructure.Repository
{
    public class TaskAssignmentRepository : ITaskAssignmentRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskAssignmentRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Add(TaskAssignment taskAssignment)
        {
            await _unitOfWork.Context.Set<TaskAssignment>().AddAsync(taskAssignment);
            _unitOfWork.Commit();
            return taskAssignment.Id;
        }

        public async Task<bool> Delete(int id)
        {
            bool isDeleted;
            try
            {
                TaskAssignment taskAssignment = await _unitOfWork.Context.TaskAssignments.Where(x => x.Id == id).FirstOrDefaultAsync();
                _unitOfWork.Context.TaskAssignments.Remove(taskAssignment);
                _unitOfWork.Commit();
                isDeleted = true;
            }
            catch
            {
                isDeleted = false;
            }
            
            return isDeleted;
        }

        public async Task<IEnumerable<TaskAssignment>> GetAll()
        {
            var taskAssignments = _unitOfWork.Context.TaskAssignments;
            return await taskAssignments.ToListAsync();

        }

        public async Task<TaskAssignment> GetById(int id)
        {
            var data = await _unitOfWork.Context.TaskAssignments.Where(x => x.Id == id).FirstOrDefaultAsync();
            return data;

        }

        public int Update(TaskAssignment taskAssignment)
        {
            int timesheetAssignmnetUpdate = 0;
            _unitOfWork.Context.Entry(taskAssignment).State = EntityState.Modified;
            _unitOfWork.Context.TaskAssignments.Update(taskAssignment);
            _unitOfWork.Commit();
            timesheetAssignmnetUpdate = taskAssignment.Id;
            return timesheetAssignmnetUpdate;
        }
    }
}
