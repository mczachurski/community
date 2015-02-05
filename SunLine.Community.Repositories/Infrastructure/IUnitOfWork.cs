namespace SunLine.Community.Repositories.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}