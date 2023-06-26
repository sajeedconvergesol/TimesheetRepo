using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Infrastructure.Interfaces;
using TMS.Infrastructure.Repository;
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

        public async Task<bool> Add(List<InvoiceDetails> invoiceDetails)
        {
            return await _InvoiceDetailRepository.Add(invoiceDetails);
        }

        public async Task<int> Delete(int id)
        {
            return await _InvoiceDetailRepository.Delete(id);

        }

        public async Task<IEnumerable<InvoiceDetails>> GetAll()
        {
            return await _InvoiceDetailRepository.GetAll();
        }

        public async Task<InvoiceDetails> GetById(int id)
        {
            return await _InvoiceDetailRepository.GetById(id);
        }
    }
}
