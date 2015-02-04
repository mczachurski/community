using System.Web.Mvc;
using SunLine.Community.Web.SessionContext;
using SunLine.Community.Web.ViewModelsServices;

namespace SunLine.Community.Web.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private readonly ISearchViewModelService _searchViewModelService;

        public SearchController(ISearchViewModelService searchViewModelService)
        {
            _searchViewModelService = searchViewModelService;
        }

        [HttpGet]
        public ActionResult Index(string s)
        {
            var watcherUserId = this.CurrentUserSessionContext().UserId;
            var searchViewModel = _searchViewModelService.CreateSearchViewModel(watcherUserId, s);
            return View(searchViewModel);
        }
    }
}