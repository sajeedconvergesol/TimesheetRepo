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
	public class TaskRepository : ITaskRepository
	{
		private readonly IUnitOfWork _unitOfWork;

		public TaskRepository(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

		}

		public async Task<int> Add(Tasks task)
		{
			_unitOfWork.Context.Task.Add(task);
			await _unitOfWork.Context.SaveChangesAsync();
			return task.Id;
		}

		public async Task<int> Delete(int id)
		{
			Tasks task = await _unitOfWork.Context.Task.FindAsync(id);
			_unitOfWork.Context.Task.Remove(task);
			await _unitOfWork.Context.SaveChangesAsync();
			return task.Id;
		}

		public async Task<IEnumerable<Tasks>> GetAll()
		{
			var data = await _unitOfWork.Context.Task.ToListAsync();
			await _unitOfWork.Context.SaveChangesAsync();
			return data;

		}

		public async Task<Tasks> GetById(int id)
		{
			var data = await _unitOfWork.Context.Task.FindAsync(id);
			await _unitOfWork.Context.SaveChangesAsync();
			return data;
		}

		public async Task<int> Update(Tasks task)
		{
			int entry = 0;
			Tasks olddata = await _unitOfWork.Context.Task.FindAsync(task.Id);
			if (olddata != null)
			{
				olddata.TaskName = task.TaskName;
				olddata.TaskDescription = task.TaskDescription;
				olddata.EstimatedHours = task.EstimatedHours;
				entry = await _unitOfWork.Context.SaveChangesAsync();
			}
			return entry;
		}
	}
}
