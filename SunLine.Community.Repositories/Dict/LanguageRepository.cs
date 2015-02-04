using System.Linq;
using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Dict
{
    public class LanguageRepository : EntityRepository<Language>, ILanguageRepository
    {
        public LanguageRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }

        public Language FindByCode(string code)
        {
            return FindAll().FirstOrDefault(x => x.Code == code);
        }
    }
}