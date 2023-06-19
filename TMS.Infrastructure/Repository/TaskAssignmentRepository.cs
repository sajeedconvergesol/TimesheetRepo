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
			_unitOfWork.Context.TaskAssignments.Add(taskAssignment);
			await _unitOfWork.Context.SaveChangesAsync();
			return taskAssignment.Id;
		}

		public async Task<int> Delete(int id)
		{
			TaskAssignment taskAssignment = await _unitOfWork.Context.TaskAssignments.FindAsync(id);
			_unitOfWork.Context.TaskAssignments.Remove(taskAssignment);
			await _unitOfWork.Context.SaveChangesAsync();
			return taskAssignment.Id;
		}

		public async Task<IEnumerable<TaskAssignment>> GetAll()
		{
			var data = await _unitOfWork.Context.TaskAssignments.ToListAsync();
			await _unitOfWork.Context.SaveChangesAsync();
			return data;

		}

		public async Task<TaskAssignment> GetById(int id)
		{
			var data = await _unitOfWork.Context.TaskAssignments.FindAsync(id);
			await _unitOfWork.Context.SaveChangesAsync();
			return data;

		}

		public async Task<int> Update(TaskAssignment taskAssignment)
		{
			int entry = 0;
			TaskAssignment olddata = await _unitOfWork.Context.TaskAssignments.FindAsync(taskAssignment.Id);
			if (olddata != null)
			{
				olddata.ProjectId = taskAssignment.ProjectId;
				olddata.EmployeeId = taskAssignment.EmployeeId;
				olddata.TaskId = taskAssignment.TaskId;
				olddata.TaskStatus = taskAssignment.TaskStatus;
				olddata.DueDate = taskAssignment.DueDate;
				olddata.CreatedOn= taskAssignment.CreatedOn;
				olddata.CreatedBy = taskAssignment.CreatedBy;
				entry = await _unitOfWork.Context.SaveChangesAsync();
			}
			return entry;
		}
	}
}
