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
	public class TimesheetDetailsRepository : ITimesheetDetailsRepository
	{
		private readonly IUnitOfWork _unitOfWork;

		public TimesheetDetailsRepository(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

		}

		public async Task<int> Add(TimeSheetDetails timesheetDetail)
		{
			await _unitOfWork.Context.TimeSheetDetails.AddAsync(timesheetDetail);
			await _unitOfWork.Context.SaveChangesAsync();
			return timesheetDetail.TimeSheetMasterId;


			//await _unitOfWork.Context.Set<TimeSheetDetails>().AddAsync(timesheetDetail);
			//return timesheetDetail.Id;
		}

		public async Task<int> Delete(int id)
		{
			TimeSheetDetails timeSheetDetails = await _unitOfWork.Context.TimeSheetDetails.FindAsync(id);
			_unitOfWork.Context.TimeSheetDetails.Remove(timeSheetDetails);
			await _unitOfWork.Context.SaveChangesAsync();
			return timeSheetDetails.Id;
		}

		public async Task<IEnumerable<TimeSheetDetails>> GetAll()
		{
		var data = await _unitOfWork.Context.TimeSheetDetails.ToListAsync();
			await _unitOfWork.Context.SaveChangesAsync();
			return data;
		}

		public async Task<TimeSheetDetails> GetById(int id)
		{
		var data = await _unitOfWork.Context.TimeSheetDetails.FindAsync(id);
			await _unitOfWork.Context.SaveChangesAsync();
			return data;
		}

		public async Task<int> Update(TimeSheetDetails timesheetDetail)
		{
			int entry = 0;
			TimeSheetDetails olddata = await _unitOfWork.Context.TimeSheetDetails.FindAsync(timesheetDetail.Id);
			if (olddata != null)
			{
				olddata.TimeSheetMasterId = timesheetDetail.TimeSheetMasterId;
				olddata.TaskAssignmentId = timesheetDetail.TaskAssignmentId;
				olddata.Period = timesheetDetail.Period;
				olddata.Hours = timesheetDetail.Hours;
				entry = await _unitOfWork.Context.SaveChangesAsync();
			}
			return entry;
		}
	}
}
