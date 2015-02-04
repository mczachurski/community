using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Dict
{
    public interface ILanguageRepository : IEntityRepository<Language>
    {
        Language FindByCode(string code);
    }
}
