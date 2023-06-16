using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Infrastructure.Context;
using TMS.Infrastructure.Interfaces;

namespace TMS.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDbContext Context { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            Context = context;
        }
        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
