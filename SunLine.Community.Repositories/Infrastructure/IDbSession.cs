namespace SunLine.Community.Repositories.Infrastructure
{
    public interface IDbSession
    {
        IDatabaseContext Current { get; }
    }
}
