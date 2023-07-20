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
			await _unitOfWork.Context.Set<Tasks>().AddAsync(task);
			_unitOfWork.Commit();
			return task.Id;
		}

		public async Task<bool> Delete(int id)
		{
			bool isDeleted;
			try
			{
                Tasks? task = await _unitOfWork.Context.Task.Where(x=>x.Id==id).FirstOrDefaultAsync();
                _unitOfWork.Context.Task.Remove(task);
				_unitOfWork.Commit();
				isDeleted = true;
            }
			catch
			{
                isDeleted = false;
            }
			
			return isDeleted;
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

		public int Update(Tasks task)
		{
            _unitOfWork.Context.Entry(task).State = EntityState.Modified;
            _unitOfWork.Context.Task.Update(task);
            _unitOfWork.Commit();
            return task.Id;
        }
	}
}
