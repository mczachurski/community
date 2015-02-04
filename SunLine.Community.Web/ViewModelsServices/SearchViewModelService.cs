using System;
using SunLine.Community.Entities.Search;
using SunLine.Community.Services.Search;
using SunLine.Community.Web.Common;
using SunLine.Community.Web.ViewModels.Search;

namespace SunLine.Community.Web.ViewModelsServices
{
    [ViewModelService]
    public class SearchViewModelService: ISearchViewModelService
    {
        private readonly ISearchService _searchService;
        private readonly IUsersViewModelService _usersViewModelService;
        private readonly IMessagesViewModelService _messagesViewModelService;

        public SearchViewModelService(ISearchService searchService, IUsersViewModelService usersViewModelService, MessagesViewModelService messagesViewModelService)
        {
            _searchService = searchService;
            _usersViewModelService = usersViewModelService;
            _messagesViewModelService = messagesViewModelService;
        }

        public SearchViewModel CreateSearchViewModel(Guid watcherUserId, string s)
        {
            UserSearchResult userSearchResult = _searchService.SearchUsers(s, 0, 20);
            var usersViewModel = _usersViewModelService.CreateUsersViewModel(userSearchResult.Users, watcherUserId);

            UserMessageSearchResult userMessageSearchResult = _searchService.SearchMessages(s, 0, 20);
            var timelineViewModel = _messagesViewModelService.CreateTimelineViewModel(null, watcherUserId, userMessageSearchResult.UserMessages);

            var viewModel = new SearchViewModel
                {
                    Query = s, 
                    AmountOfUsers = userSearchResult.AllResults,
                    AmountOfMessages = userMessageSearchResult.AllResults,
                    Users = usersViewModel,
                    TimelineViewModel = timelineViewModel
                };

            return viewModel;
        }
    }
}
