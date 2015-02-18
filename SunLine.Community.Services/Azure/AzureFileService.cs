using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SunLine.Community.Services.Core;

namespace SunLine.Community.Services.Azure
{
    [BusinessLogic]
    public class AzureFileService : IAzureFileService
    {
        private readonly ISettingService _settingService;
        private const string AzuerStorageHostAddress = "http://minds.blob.core.windows.net";
        private const string ThumbnailsDirectory = "thumbnails";

        public AzureFileService(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public void SaveFile(string userName, string filePath, Stream fileStream)
        {
            CloudStorageAccount storageAccount = GetCloudStorageAccount();
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            string lowerCaseUserName = userName.ToLower();
            CloudBlobContainer container = blobClient.GetContainerReference(lowerCaseUserName);

            if (!container.Exists())
            {
                container.Create();
                container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }); 
            }

            string lowerCaseFilePath = filePath.ToLower();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(lowerCaseFilePath);
            blockBlob.UploadFromStream(fileStream);
        }

        public CloudStorageAccount GetCloudStorageAccount()
        {
            string accountConnectionString = _settingService.StorageConnectionString;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(accountConnectionString);

            return storageAccount;
        }

        public string GetUrlToFile(string userName, string filePath)
        {
            string lowerCaseUserName = userName.ToLower();
            string lowerCaseFilePath = filePath.ToLower();
            return string.Format("{0}/{1}/{2}", AzuerStorageHostAddress, lowerCaseUserName, lowerCaseFilePath);
        }

        public string GetUrlToFileThumbnail(string userName, string filePath)
        {
            string lowerCaseUserName = userName.ToLower();
            string lowerCaseFilePath = filePath.ToLower();
            return string.Format("{0}/{1}/{2}/{3}", AzuerStorageHostAddress, lowerCaseUserName, ThumbnailsDirectory, lowerCaseFilePath);
        }
    }
}
