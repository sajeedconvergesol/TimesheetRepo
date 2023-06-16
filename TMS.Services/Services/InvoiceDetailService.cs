using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Infrastructure.Interfaces;
using TMS.Services.Interfaces;

namespace TMS.Services.Services
{
    public class InvoiceDetailService : IInvoiceDetailService
    {
        public readonly IInvoiceDetailRepository _InvoiceDetailRepository;
        public InvoiceDetailService(IInvoiceDetailRepository invoiceDetailRepository)
        {
            _InvoiceDetailRepository = invoiceDetailRepository;
        }
        public async Task<long> AddInvoiceDetails(InvoiceDetails invoiceDetails)
        {
            await _InvoiceDetailRepository.AddInvoiceDetails(invoiceDetails);
            return invoiceDetails.Id;
        }
    }
}
