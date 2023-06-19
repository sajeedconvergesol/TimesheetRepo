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
        Task<long> Add(InvoiceDetails invoiceDetails);
        Task<long> Update(InvoiceDetails invoiceDetails);
        Task<long> Delete(int id);
    }
}
