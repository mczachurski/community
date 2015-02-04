using System.Linq;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Core
{
    public class HashtagRepository : EntityRepository<Hashtag>, IHashtagRepository
    {
        public HashtagRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }

        public Hashtag FindByName(string hashtag)
        {
            return FindAll().FirstOrDefault(x => x.Name == hashtag);
        }
    }
}