using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Services.Infrastructure
{
    public interface IDatabaseTransactionService
    {
        DatabaseTransaction BeginTransaction();
    }
    
}
