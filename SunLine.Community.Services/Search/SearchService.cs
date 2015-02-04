using System.Linq;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store.Azure;
using Microsoft.WindowsAzure.Storage;
using SunLine.Community.Entities.Core;
using SunLine.Community.Entities.Dict;
using SunLine.Community.Entities.Search;
using SunLine.Community.Repositories.Core;
using SunLine.Community.Services.Azure;

namespace SunLine.Community.Services.Search
{
    [BusinessLogic]
    public class SearchService : ISearchService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserMessageRepository _userMessageRepository;
        private readonly IAzureFileService _azureFileService;

        public SearchService(
            IUserRepository userRepository, 
            IUserMessageRepository userMessageRepository,
            IAzureFileService azureFileService)
        {
            _userRepository = userRepository;
            _userMessageRepository = userMessageRepository;
            _azureFileService = azureFileService;
        }

        public UserSearchResult SearchUsers(string s, int page, int amountOnPage)
        {
            IQueryable<User> allFoundQuery = _userRepository.FindAll()
                .Where(x => x.FirstName.Contains(s) || x.LastName.Contains(s) || x.UserName.Contains(s));

            int allResults = allFoundQuery.Count();
            IQueryable<User> partResultQuery =  allFoundQuery.OrderBy(x => x.CreationDate).Skip(page * amountOnPage).Take(amountOnPage);

            var userSearchResult = new UserSearchResult {
                AmountOnPage = amountOnPage,
                Page = page,
                AllResults = allResults,
                Users = partResultQuery.ToList()
            };

            return userSearchResult;
        }
            
        public UserMessageSearchResult SearchMessages(string s, int page, int amountOnPage)
        {
            IQueryable<UserMessage> allFoundQuery = _userMessageRepository.FindAll()
                .Where(x => x.UserMessageState.UserMessageStateEnum != UserMessageStateEnum.Deleted
                    && x.Message.MessageState.MessageStateEnum == MessageStateEnum.Published
                    && x.UserMessageCreationMode.UserMessageCreationModeEnum == UserMessageCreationModeEnum.ByHimselfNew
                    && (x.Message.Mind.Contains(s) || x.Message.Speech.Contains(s)));

            int allResults = allFoundQuery.Count();
            IQueryable<UserMessage> partResultQuery =  allFoundQuery.OrderBy(x => x.CreationDate).Skip(page * amountOnPage).Take(amountOnPage);

            var userMessageSearchResult = new UserMessageSearchResult {
                AmountOnPage = amountOnPage,
                Page = page,
                AllResults = allResults,
                UserMessages = partResultQuery.ToList()
            };

            return userMessageSearchResult;
        }

        public void AddMessageToIndex(Message message)
        {
            using (var indexWriter = CreateIndexWriter())
            {
                var document = new Document();

                document.Add(new Field("Id", 
                    message.Id.ToString(), 
                    Field.Store.YES, 
                    Field.Index.NO, 
                    Field.TermVector.NO));

                document.Add(new Field("Mind", 
                    message.Mind ?? string.Empty, 
                    Field.Store.YES, 
                    Field.Index.ANALYZED, 
                    Field.TermVector.NO));

                document.Add(new Field("Speech", 
                    message.Speech ?? string.Empty, 
                    Field.Store.YES, 
                    Field.Index.ANALYZED, 
                    Field.TermVector.NO));

                indexWriter.AddDocument(document);
            }
        }

        public void AddUserToIndex(User user)
        {
            using (var indexWriter = CreateIndexWriter())
            {
                var document = new Document();

                document.Add(new Field("Id", 
                    user.Id.ToString(), 
                    Field.Store.YES, 
                    Field.Index.NO, 
                    Field.TermVector.NO));

                document.Add(new Field("FullName", 
                    user.FullName ?? string.Empty, 
                    Field.Store.YES, 
                    Field.Index.ANALYZED, 
                    Field.TermVector.NO));

                document.Add(new Field("UserName", 
                    user.UserName ?? string.Empty, 
                    Field.Store.YES, 
                    Field.Index.ANALYZED, 
                    Field.TermVector.NO));

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