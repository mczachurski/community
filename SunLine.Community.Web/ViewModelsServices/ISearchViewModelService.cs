using System;
using SunLine.Community.Web.ViewModels.Search;

namespace SunLine.Community.Web.ViewModelsServices
{
    public interface ISearchViewModelService
    {
        SearchViewModel CreateSearchViewModel(Guid watcherUserId, string s);
    }
}

