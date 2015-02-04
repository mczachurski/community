using System;
using System.IO;
using SunLine.Community.Entities.Core;
using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Core;
using SunLine.Community.Repositories.Dict;
using SunLine.Community.Services.Azure;
using SunLine.Community.Common;
using File = SunLine.Community.Entities.Core.File;

namespace SunLine.Community.Services.Core
{
    [BusinessLogic]
    public class FileService : IFileService
    {
        private readonly IAzureFileService _azureFileService;
        private readonly IFileRepository _fileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFileTypeRepository _fileTypeRepository;

        public FileService(
            IAzureFileService azureFileService,
            IFileRepository fileRepository, 
            IUserRepository userRepository, 
            IFileTypeRepository fileTypeRepository)
        {
            _azureFileService = azureFileService;
            _fileRepository = fileRepository;
            _userRepository = userRepository;
            _fileTypeRepository = fileTypeRepository;
        }

        public File Create(Guid userId, string fileName, string contentType, int contentLength, System.IO.Stream fileStream)
        {
            User user = _userRepository.FindById(userId);

            string path = string.Format("{0}/{1}", DateTime.UtcNow.Ticks, fileName);
            var file = new File
            {
                    User = user,
                    Path = path,
                    ContentLength = contentLength,
                    ContentType = contentType,
                    FileType = GetFileType(contentType),
                    Identifier = 1
            };

            file = _fileRepository.Create(file);
            _azureFileService.SaveFile(user.UserName, path, fileStream);

            if(file.FileType.FileTypeEnum == FileTypeEnum.Image)
            {
                CreateThumbnail(file, fileStream);
            }

            return file;
        }

        public string GetUrlToFile(File file)
        {
            return _azureFileService.GetUrlToFile(file.User.UserName, file.Path);
        }

        public string GetUrlToFileThumbnail(File file)
        {
            return _azureFileService.GetUrlToFileThumbnail(file.User.UserName, file.Path);
        }

        private void CreateThumbnail(File file, System.IO.Stream fileStream)
        {
            const int thumbnailWidth = 1300;
            using (Stream thumbnailStream = ImageHelper.GetThumbnail(fileStream, thumbnailWidth))
            {
                string thumbnailPath = string.Format("thumbnails/{0}", file.Path);
                thumbnailStream.Seek(0, SeekOrigin.Begin);
                _azureFileService.SaveFile(file.User.UserName, thumbnailPath, thumbnailStream);
            }
        }

        private FileType GetFileType(string contentType)
        {
            if(contentType.Equals("image/gif", StringComparison.CurrentCultureIgnoreCase))
            {
                return _fileTypeRepository.FindByEnum(FileTypeEnum.Image);
            }
            
            if(contentType.StartsWith("image", StringComparison.CurrentCultureIgnoreCase))
            {
                return _fileTypeRepository.FindByEnum(FileTypeEnum.Image);
            }
            
            if(contentType.StartsWith("video", StringComparison.CurrentCultureIgnoreCase))
            {
                return _fileTypeRepository.FindByEnum(FileTypeEnum.Video);
            }

            throw new ArgumentException(string.Format("Unsupported file type: {0}", contentType));
        }
    }
}
