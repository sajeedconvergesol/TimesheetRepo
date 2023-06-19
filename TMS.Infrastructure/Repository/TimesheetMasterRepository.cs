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
	public class TimesheetMasterRepository : ITimesheetMasterRepository
	{
		private readonly IUnitOfWork _unitOfWork;
		public TimesheetMasterRepository(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<int> CreateTimesheetMaster(TimeSheetMaster timesheetMaster)
		{	
			await _unitOfWork.Context.TimeSheetMaster.AddAsync(timesheetMaster);
			await _unitOfWork.Context.SaveChangesAsync();
			return timesheetMaster.Id;

		}

		public async Task<int> DeleteTimesheetMaster(int id)
		{
			TimeSheetMaster timesheetMaster = await _unitOfWork.Context.TimeSheetMaster.FindAsync(id);
			_unitOfWork.Context.TimeSheetMaster.Remove(timesheetMaster);
			await _unitOfWork.Context.SaveChangesAsync();
			return timesheetMaster.Id;
		}

		public async Task<TimeSheetMaster> GetTimesheetMaster(int id)
		{
			var data = await _unitOfWork.Context.TimeSheetMaster.FindAsync(id);
			await _unitOfWork.Context.SaveChangesAsync();
			return data;

		}

		public async Task<IEnumerable<TimeSheetMaster>> GetTimesheetMastersAll()
		{ 
			var data = await _unitOfWork.Context.TimeSheetMaster.ToListAsync();
			await _unitOfWork.Context.SaveChangesAsync();
			return data;
		}

		public async Task<int> UpdateTimesheetMaster(TimeSheetMaster timesheetMaster)
		{
			int entry = 0;
		   TimeSheetMaster olddata = await _unitOfWork.Context.TimeSheetMaster.FindAsync(timesheetMaster.Id);
			if(olddata != null)
			{
				olddata.FromDate = timesheetMaster.FromDate;
				olddata.ToDate = timesheetMaster.ToDate;
				olddata.TotalHours = timesheetMaster.TotalHours;
				olddata.TimeSheetStatus = timesheetMaster.TimeSheetStatus;
				olddata.Comments = timesheetMaster.Comments;
				olddata.CreatedOn = timesheetMaster.CreatedOn;
				olddata.CreatedBy = timesheetMaster.CreatedBy;
				olddata.LastModifiedOn = timesheetMaster.LastModifiedOn;
				olddata.LastModifiedBy = timesheetMaster.LastModifiedBy;
				entry = await _unitOfWork.Context.SaveChangesAsync();
			}
			return entry;
		}
	}
}
