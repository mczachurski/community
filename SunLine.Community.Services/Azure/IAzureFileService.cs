using System.IO;
using Microsoft.WindowsAzure.Storage;

namespace SunLine.Community.Services.Azure
{
    public interface IAzureFileService
    {
        void SaveFile(string userName, string filePath, Stream fileStream);
        CloudStorageAccount GetCloudStorageAccount();
        string GetUrlToFile(string userName, string filePath);
        string GetUrlToFileThumbnail(string userName, string filePath);
    }
}

