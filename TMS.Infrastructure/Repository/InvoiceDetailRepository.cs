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
    public class InvoiceDetailRepository : IInvoiceDetailRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public InvoiceDetailRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Add(InvoiceDetails invoiceDetails)
        {
            _unitOfWork.Context.InvoiceDetails.Add(invoiceDetails);
            await _unitOfWork.Context.SaveChangesAsync();
            return invoiceDetails.Id;
        }

        public async Task<int> Delete(int id)
        {
            InvoiceDetails invoiceDetails = await _unitOfWork.Context.InvoiceDetails.FindAsync(id);
            _unitOfWork.Context.InvoiceDetails.Remove(invoiceDetails);
            await _unitOfWork.Context.SaveChangesAsync();
            return id;
        }

        public async Task<IEnumerable<InvoiceDetails>> GetAll()
        {
            var data = await _unitOfWork.Context.InvoiceDetails.ToListAsync();
            await _unitOfWork.Context.SaveChangesAsync();
            return data;
        }

        public async Task<InvoiceDetails> GetById(int id)
        {
            var data = await _unitOfWork.Context.InvoiceDetails.FindAsync();
            await _unitOfWork.Context.SaveChangesAsync();
            return data;
        }

        public async Task<int> Update(InvoiceDetails invoiceDetails)
        {
            int entry = 0;
            InvoiceDetails olddata = await _unitOfWork.Context.InvoiceDetails.FindAsync(invoiceDetails.Id);
            if (olddata != null)
            {
                olddata.HoursBilled = invoiceDetails.HoursBilled;
                olddata.RatePerHour = invoiceDetails.RatePerHour;
                olddata.TaskAssignmentId = invoiceDetails.TaskAssignmentId;
                entry = await _unitOfWork.Context.SaveChangesAsync();
            }
            return entry;
        }
    }
}
