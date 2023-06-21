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
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

            public Task<int> Add(Invoice invoice)
        {
            return _invoiceRepository.Add(invoice);
        }

        public Task<int> Delete(int id)
        {
            return _invoiceRepository.Delete(id);
        }

        public Task<IEnumerable<Invoice>> GetAll()
        {
            return _invoiceRepository.GetAll();
        }

        public Task<Invoice> GetById(int id)
        {
            return _invoiceRepository.GetById(id);
        }

        public Task<int> Update(Invoice invoice)
        {
            return _invoiceRepository.Update(invoice);
        }
    }
}
