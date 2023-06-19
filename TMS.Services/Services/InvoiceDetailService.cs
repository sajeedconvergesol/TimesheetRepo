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

        public Task<long> Add(InvoiceDetails invoiceDetails)
        {
           return _InvoiceDetailRepository.Add(invoiceDetails);
        }

        public Task<long> Delete(int id)
        {
            return _InvoiceDetailRepository.Delete(id);

        }

        public Task<IEnumerable<InvoiceDetails>> GetAll()
        {
            return _InvoiceDetailRepository.GetAll();

        }

        public Task<InvoiceDetails> GetById(int id)
        {
            return _InvoiceDetailRepository.GetById(id);

        }

        public Task<long> Update(InvoiceDetails invoiceDetails)
        {
            return _InvoiceDetailRepository.Update(invoiceDetails);
        }
    }
}
