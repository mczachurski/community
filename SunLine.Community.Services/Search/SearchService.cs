using System.Linq;
using Lucene.Net.Documents;
using SunLine.Community.Entities.Core;
using SunLine.Community.Entities.Dict;
using SunLine.Community.Entities.Search;
using SunLine.Community.Repositories.Core;

namespace SunLine.Community.Services.Search
{
    [BusinessLogic]
    public class SearchService : ISearchService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserMessageRepository _userMessageRepository;
        private readonly ILuceneService _luceneService;

        public SearchService(
            IUserRepository userRepository, 
            IUserMessageRepository userMessageRepository,
            ILuceneService luceneService)
        {
            _userRepository = userRepository;
            _userMessageRepository = userMessageRepository;
            _luceneService = luceneService;
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

            _luceneService.AddDocument(document);
        }

        public void AddUserToIndex(User user)
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

            _luceneService.AddDocument(document);
        }
    }
}