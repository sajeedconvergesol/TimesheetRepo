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
            await _unitOfWork.Context.Set<Invoice>().AddAsync(invoice);
            _unitOfWork.Commit();
            return invoice.Id;
        }

        public async Task<bool> Delete(int id)
        {
            bool isDeleted;
            try
            {
                Invoice invoiceDetails = await _unitOfWork.Context.Invoices.Where(x => x.Id == id).FirstOrDefaultAsync();
                _unitOfWork.Context.Invoices.Remove(invoiceDetails);
                
                isDeleted = true;
            }
            catch
            {
                isDeleted = false;
            }
            
            return isDeleted;
        }

        public async Task<IEnumerable<Invoice>> GetAll()
        {
            var data = await _unitOfWork.Context.Invoices.ToListAsync();
            await _unitOfWork.Context.SaveChangesAsync();
            return data;
        }

        public async Task<Invoice> GetById(int id)
        {
            var data = await _unitOfWork.Context.Invoices.Where(x => x.Id == id).FirstOrDefaultAsync();
            return data;
        }

        public int Update(Invoice invoice)
        {
            _unitOfWork.Context.Entry(invoice).State = EntityState.Modified;
            _unitOfWork.Context.Invoices.Update(invoice);
            _unitOfWork.Commit();
            return invoice.Id;
        }
    }
}
