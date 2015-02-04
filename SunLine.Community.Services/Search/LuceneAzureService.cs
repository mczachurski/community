using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store.Azure;
using Microsoft.WindowsAzure.Storage;
using SunLine.Community.Services.Azure;

namespace SunLine.Community.Services.Search
{
    public class LuceneAzureService : ILuceneService
    {
        private readonly IAzureFileService _azureFileService;

        public LuceneAzureService(IAzureFileService azureFileService)
        {
            _azureFileService = azureFileService;
        }

        public void AddDocument(Document document)
        {
            using (var indexWriter = CreateIndexWriter())
            {
                indexWriter.AddDocument(document);
            }
        }

        private IndexWriter CreateIndexWriter()
        {
            CloudStorageAccount storageAccount = _azureFileService.GetCloudStorageAccount();

            const string systemAzureLuceneIndexContainerName = "SYSAzureLuceneIndexContainer";
            var azureDir = new AzureDirectory(storageAccount, systemAzureLuceneIndexContainerName);

            var indexWriter = new IndexWriter(azureDir, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30), true,
                new IndexWriter.MaxFieldLength(IndexWriter.DEFAULT_MAX_FIELD_LENGTH));

            return indexWriter;
        }
    }
}