using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Services.Infrastructure
{
    [BusinessLogic]
    public class DatabaseTransactionService : IDatabaseTransactionService
    {
        private readonly IDbSession _dbSession;

        public DatabaseTransactionService(IDbSession dbSession)
        {
            _dbSession = dbSession;
        }

        public DatabaseTransaction BeginTransaction()
        {
            return new DatabaseTransaction(_dbSession);
        }
    }
}

