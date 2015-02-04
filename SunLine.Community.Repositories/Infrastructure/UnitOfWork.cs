using System.Data.Entity;

namespace SunLine.Community.Repositories.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbSession _dbSession;

        public UnitOfWork(IDbSession dbSession)
        {
            _dbSession = dbSession;
        }

        public void Commit()
        {
            _dbSession.Current.Commit();
        }
    }
}
