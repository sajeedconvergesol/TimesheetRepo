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
			await _unitOfWork.Context.Set<TimeSheetMaster>().AddAsync(timesheetMaster);
			_unitOfWork.Commit();
			return timesheetMaster.Id;

		}

		public async Task<bool> DeleteTimesheetMaster(int id)
		{
			bool isDeleted;
			try
			{
                TimeSheetMaster? timesheetMaster = await _unitOfWork.Context.TimeSheetMaster.Where(x => x.Id == id).FirstOrDefaultAsync();
                _unitOfWork.Context.TimeSheetMaster.Remove(timesheetMaster);
                _unitOfWork.Commit();
                isDeleted = true;
            }
			catch
			{
				isDeleted = false;
			}
			
			return isDeleted;
		}

		public async Task<TimeSheetMaster> GetTimesheetMaster(int id)
		{
			var data = _unitOfWork.Context.TimeSheetMaster.Where(x => x.Id == id).FirstOrDefaultAsync();
            return await data;
		}

		public async Task<IEnumerable<TimeSheetMaster>> GetTimesheetMastersAll()
		{
			var data = _unitOfWork.Context.TimeSheetMaster;
			return await data.ToListAsync();
        }

		public int UpdateTimesheetMaster(TimeSheetMaster timesheetMaster)
		{
            int timesheetMasterUpdate = 0;
            _unitOfWork.Context.Entry(timesheetMaster).State = EntityState.Modified;
            _unitOfWork.Context.TimeSheetMaster.Update(timesheetMaster);
            _unitOfWork.Commit();
			timesheetMasterUpdate = timesheetMaster.Id;
            return timesheetMasterUpdate;
        }
        public async Task<IEnumerable<TimeSheetMaster>> GetByUserId(int userId)
        {
            var data = _unitOfWork.Context.TimeSheetMaster.Where(x => x.CreatedBy == userId);
            return await data.ToListAsync();
        }
    }
}
