using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;

namespace TMS.Infrastructure.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<IEnumerable<Invoice>> GetAll();
        Task<Invoice> GetById(int id);
        Task<int> Add(Invoice invoice);
        int Update(Invoice invoice);
        Task<bool> Delete(int id);
    }
}
