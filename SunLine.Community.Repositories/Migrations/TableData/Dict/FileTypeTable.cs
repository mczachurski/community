using System;
using System.Linq;
using SunLine.Community.Entities.Dict;

namespace SunLine.Community.Repositories.Migrations.TableData.Dict
{
    public static class FileTypeTable
    {
        public static void Initialize(IDatabaseContext context)
        {
            AddFileType(context, FileTypeEnum.Image, "Image");
            AddFileType(context, FileTypeEnum.Video, "Video");
            AddFileType(context, FileTypeEnum.Gif, "Gif");
        }

        private static void AddFileType(IDatabaseContext context, FileTypeEnum fileType, string name)
        {
            if (!context.FileTypes.Any(x => x.FileTypeEnum == fileType))
            {
                context.FileTypes.Add(new FileType { Name = name, FileTypeEnum = fileType, CreationDate = DateTime.UtcNow, Version = 1 });
            }
        }
    }
}
