using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Infrastructure;
using System.Linq;

namespace SunLine.Community.Repositories.Dict
{
    public class FileTypeRepository  : EntityRepository<FileType>, IFileTypeRepository
    {
        public FileTypeRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }

        public FileType FindByEnum(FileTypeEnum fileTypeEnum)
        {
            return FindAll().FirstOrDefault(x => x.FileTypeEnum == fileTypeEnum);
        }
    }
}