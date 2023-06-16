using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Infrastructure.Context;

namespace TMS.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationDbContext Context { get; }
        void Commit();
    }
}
