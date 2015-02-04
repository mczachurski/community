using System.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SunLine.Community.Services.Core;

namespace SunLine.Community.Services.Azure
{
    [BusinessLogic]
    public class AzureFileService : IAzureFileService
    {
        private readonly ISettingService _settingService;

        public AzureFileService(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public void SaveFile(string userName, string filePath, Stream fileStream)
        {
            CloudStorageAccount storageAccount = GetCloudStorageAccount();
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(userName);

            if (!container.Exists())
            {
                container.Create();
                container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }); 
            }

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filePath);
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
            return string.Format("http://minds.blob.core.windows.net/{0}/{1}", userName, filePath);
        }

        public string GetUrlToFileThumbnail(string userName, string filePath)
        {
            return string.Format("http://minds.blob.core.windows.net/{0}/thumbnails/{1}", userName, filePath);
        }
    }
}
