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

        public async Task<bool> Add(List<TimeSheetDetails> timesheetDetailList)
        {
            bool isSuccess = false;
            if (timesheetDetailList.Any())
            {
                await _unitOfWork.Context.TimeSheetDetails.AddRangeAsync(timesheetDetailList);
                _unitOfWork.Commit();
                isSuccess = true;
            }
            return isSuccess;
        }

        public async Task<bool> Delete(int id)
        {
            bool isDeleted = false;
            try
            {
                TimeSheetDetails timeSheetDetails = await _unitOfWork.Context.TimeSheetDetails.Where(x => x.Id == id).FirstOrDefaultAsync();
                _unitOfWork.Context.TimeSheetDetails.Remove(timeSheetDetails);
                _unitOfWork.Commit();
                isDeleted = true;
            }
            catch (Exception)
            {
                isDeleted = false;
            }
            return isDeleted;
        }

        public async Task<IEnumerable<TimeSheetDetails>> GetAll()
        {
            var data = _unitOfWork.Context.TimeSheetDetails;
            return await data.ToListAsync();
        }

        public async Task<TimeSheetDetails> GetById(int id)
        {
            var data = await _unitOfWork.Context.TimeSheetDetails.FindAsync(id);
            await _unitOfWork.Context.SaveChangesAsync();
            return data;
        }

        public int Update(TimeSheetDetails timesheetDetail)
        {
            int timesheetDetailUpdate = 0;
            _unitOfWork.Context.Entry(timesheetDetail).State = EntityState.Modified;
            _unitOfWork.Context.TimeSheetDetails.Update(timesheetDetail);
            _unitOfWork.Commit();
            timesheetDetailUpdate = timesheetDetail.Id;
            return timesheetDetailUpdate;
        }
        public async Task<IEnumerable<TimeSheetDetails>> GetByTimeSheetMasterId(int id)
        {
            var timeSheetDetails = _unitOfWork.Context.TimeSheetDetails.Where(x => x.TimeSheetMasterId == id);
            return await timeSheetDetails.ToListAsync();
        }
        public bool DeleteByTimeSheetId(int timesheetId)
        {
            bool isDeleted = false;
            try
            {
                var timeSheetDetails = _unitOfWork.Context.TimeSheetDetails.Where(x => x.TimeSheetMasterId == timesheetId);
                _unitOfWork.Context.TimeSheetDetails.RemoveRange(timeSheetDetails);
                _unitOfWork.Commit();
                isDeleted = true;
            }
            catch (Exception)
            {
                isDeleted = false;
            }
            return isDeleted;
        }
    }
}
