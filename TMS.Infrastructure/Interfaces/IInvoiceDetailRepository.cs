using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;

namespace TMS.Infrastructure.Interfaces
{
    public interface IInvoiceDetailRepository
    {
        Task<long> AddInvoiceDetails(InvoiceDetails invoiceDetails);
    }
}
