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
        public async Task<long> AddInvoiceDetails(InvoiceDetails invoiceDetails)
        {
            await _unitOfWork.Context.Set<InvoiceDetails>().AddAsync(invoiceDetails);
            return invoiceDetails.Id;
        }
    }
}
