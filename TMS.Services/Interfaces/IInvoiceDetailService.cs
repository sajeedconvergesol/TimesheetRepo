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
        Task<long> AddInvoiceDetails(InvoiceDetails invoiceDetails);
    }
}
