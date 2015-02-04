using System;
using SunLine.Community.Entities.Core;

namespace SunLine.Community.Services.Core
{
    public interface IFileService
    {
        File Create(Guid userId, string fileName, string contentType, int contentLength, System.IO.Stream fileStream);
        string GetUrlToFile(File file);
        string GetUrlToFileThumbnail(File file);
    }
}

