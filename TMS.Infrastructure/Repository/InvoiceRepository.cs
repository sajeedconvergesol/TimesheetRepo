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
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Add(Invoice invoice)
        {
            _unitOfWork.Context.Invoices.Add(invoice);
            await _unitOfWork.Context.SaveChangesAsync();
            return invoice.Id;
        }

        public async Task<int> Delete(int id)
        {
            Invoice invoiceDetails = await _unitOfWork.Context.Invoices.FindAsync(id);
            _unitOfWork.Context.Invoices.Remove(invoiceDetails);
            await _unitOfWork.Context.SaveChangesAsync();
            return id;
        }

        public async Task<IEnumerable<Invoice>> GetAll()
        {
            var data = await _unitOfWork.Context.Invoices.ToListAsync();
            await _unitOfWork.Context.SaveChangesAsync();
            return data;
        }

        public async Task<Invoice> GetById(int id)
        {
            var data = await _unitOfWork.Context.Invoices.FindAsync();
            await _unitOfWork.Context.SaveChangesAsync();
            return data;
        }

        public async Task<int> Update(Invoice invoice)
        {
            int entry = 0;
            Invoice olddata = await _unitOfWork.Context.Invoices.FindAsync(invoice.Id);
            if (olddata != null)
            {
                olddata.InvoiceDate = invoice.InvoiceDate;
                olddata.TimeSheetMasterId = invoice.TimeSheetMasterId;
                olddata.TotalAmount = invoice.TotalAmount;
                entry = await _unitOfWork.Context.SaveChangesAsync();
            }
            return entry;
        }
    }
}
