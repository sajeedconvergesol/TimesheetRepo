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

        public async Task<bool> Add(List<InvoiceDetails> invoiceDetails)
        {
            bool isAdd;
            try
            {
                _unitOfWork.Context.InvoiceDetails.AddRangeAsync(invoiceDetails);
                _unitOfWork.Commit();
                isAdd = true;
            }
            catch
            {
                isAdd = false;
            }
            return isAdd;
        }

        public async Task<int> Delete(int id)
        {
            InvoiceDetails invoiceDetails = await _unitOfWork.Context.InvoiceDetails.FindAsync(id);
            _unitOfWork.Context.InvoiceDetails.Remove(invoiceDetails);
            _unitOfWork.Commit();
            return id;
        }

        public async Task<IEnumerable<InvoiceDetails>> GetAll()
        {
            var data = _unitOfWork.Context.InvoiceDetails;
            return await data.ToListAsync();
        }

        public async Task<InvoiceDetails> GetById(int id)
        {
            var invoiceDetails = await _unitOfWork.Context.InvoiceDetails.Where(x=> x.Id==id).FirstOrDefaultAsync();
            return invoiceDetails;
        }
    }
}
