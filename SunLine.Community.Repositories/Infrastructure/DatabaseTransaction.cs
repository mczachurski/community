using System;
using System.Data.Entity;

namespace SunLine.Community.Repositories.Infrastructure
{
    public class DatabaseTransaction : IDisposable
    {
        private readonly IDbSession _dbSession;
        private DbContextTransaction _dbContextTransaction;
        private bool _isDisposed;

        public DatabaseTransaction(IDbSession dbSession)
        {
            _dbSession = dbSession;

            ModelConfiguration.SuspendExecutionStrategy = true;
            _dbContextTransaction = _dbSession.Current.BeginTransaction();
        }

        public void Commit()
        {
            _dbContextTransaction.Commit();
        }

        public void Rollback()
        {
            _dbContextTransaction.Rollback();
        }

        ~DatabaseTransaction()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing)
            {
                if (_dbContextTransaction != null)
                {
                    _dbContextTransaction.Dispose();
                    _dbContextTransaction = null;
                }

                ModelConfiguration.SuspendExecutionStrategy = false;
            }

            _isDisposed = true;
        }
    }
}

