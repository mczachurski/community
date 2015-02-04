using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Core
{
    public interface IHashtagRepository : IEntityRepository<Hashtag>
    {
        Hashtag FindByName(string hashtag);
    }
}
