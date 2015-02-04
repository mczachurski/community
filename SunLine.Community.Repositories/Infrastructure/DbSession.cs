using System;

namespace SunLine.Community.Repositories.Infrastructure
{
    public class DbSession : IDbSession, IDisposable
    {
        private IDatabaseContext _databaseContext;
        private bool _isDisposed;

        public IDatabaseContext Current
        {
            get
            {
                return _databaseContext ?? (_databaseContext = new DatabaseContext());
            }
        }
            
        ~DbSession()
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
                if (_databaseContext != null)
                {
                    _databaseContext.Dispose();
                    _databaseContext = null;
                }
            }

            _isDisposed = true;
        }
    }
}
