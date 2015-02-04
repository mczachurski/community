using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Dict
{
    public interface IFileTypeRepository :  IEntityRepository<FileType>
    {
        FileType FindByEnum(FileTypeEnum fileTypeEnum);
    }
}
