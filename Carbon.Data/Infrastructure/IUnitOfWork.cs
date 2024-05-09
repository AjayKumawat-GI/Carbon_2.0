using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carbon.Database.Infrastructure
{
    public interface IUnitOfWork
    {
        IDbContextTransaction BeginTransaction();
        void CommitTransaction();
        Task<int> SaveChangesAsync();
    }
}
