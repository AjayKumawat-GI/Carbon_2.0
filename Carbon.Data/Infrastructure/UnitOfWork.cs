using Carbon.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carbon.Database.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction _transaction;
        public CarbonDbContext _context;

        public UnitOfWork(
            CarbonDbContext context
            )
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            if(_transaction != null)
                throw new InvalidOperationException("A transaction is already in progress.");

            _transaction = _context.Database.BeginTransaction();
            return _transaction;
        }

        public void CommitTransaction()
        {
            if (_transaction == null)
                throw new InvalidOperationException("No transaction is in progress.");

            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }
    }
}   
