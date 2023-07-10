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

        public async Task<int> Add(Invoice invoice)
        {
            return await _invoiceRepository.Add(invoice);
        }

        public async Task<bool> Delete(int id)
        {
            return await _invoiceRepository.Delete(id);
        }

        public Task<IEnumerable<Invoice>> GetAll()
        {
            return _invoiceRepository.GetAll();
        }

        public async Task<Invoice> GetById(int id)
        {
            return await _invoiceRepository.GetById(id);
        }

        public async Task<Invoice> GetInvoiceByTimeSheetId(int id)
        {
            return await _invoiceRepository.GetInvoiceByTimeSheetId(id);
        }

        public int Update(Invoice invoice)
        {
            return _invoiceRepository.Update(invoice);
        }
    }
}
