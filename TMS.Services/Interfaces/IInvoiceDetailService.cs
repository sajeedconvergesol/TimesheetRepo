using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;

namespace TMS.Services.Interfaces
{
    public interface IInvoiceDetailService
    {
        Task<IEnumerable<InvoiceDetails>> GetAll();
        Task<InvoiceDetails> GetById(int id);
        Task<bool> Add(List<InvoiceDetails> invoiceDetails);
        Task<int> Delete(int id);
    }
}
