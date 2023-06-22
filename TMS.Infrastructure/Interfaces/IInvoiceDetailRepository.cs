using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;

namespace TMS.Infrastructure.Interfaces
{
    public interface IInvoiceDetailRepository
    {
        Task<IEnumerable<InvoiceDetails>> GetAll();
        Task<InvoiceDetails> GetById(int id);
        Task<bool> Add(List<InvoiceDetails> invoiceDetails);
        Task<int> Delete(int id);
    }
}
