using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SunLine.Community.Common;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Infrastructure;
using SunLine.Community.Services.Core;
using SunLine.Community.Web.SessionContext;
using SunLine.Community.Web.ViewModels.Messages;
using SunLine.Community.Web.ViewModelsServices;
using SunLine.Community.Web.Common;

namespace SunLine.Community.Web.Controllers
{
    [Authorize]
    public class SpeechesController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISpeechesViewModelService _speechesViewModelService;
        private readonly IMessagesViewModelService _messagesViewModelService;

        public SpeechesController(
            ISpeechesViewModelService speechesViewModelService, 
            IMessagesViewModelService messagesViewModelService,
            IMessageService messageService, 
            IUnitOfWork unitOfWork)
        {
            _speechesViewModelService = speechesViewModelService;
            _messagesViewModelService = messagesViewModelService;
            _messageService = messageService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var currentUserId = this.CurrentUserSessionContext().UserId;
            IList<SpeechViewModel> speechesViewModel = _speechesViewModelService.CreateSpeechesViewModel(currentUserId);
            return View(speechesViewModel);
        }

        [HttpGet]
        public ActionResult Show(Guid id)
        {
            var currentUserId = this.CurrentUserSessionContext().UserId;
            Message message = _messageService.FindById(id);
            MessageViewModel messageViewModel = _messagesViewModelService.CreateMessageViewModel(message, currentUserId, currentUserId);

            return View(messageViewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            SpeechViewModel speechViewModel = _speechesViewModelService.CreateSpeechViewModel();
            return View(speechViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SpeechViewModel model)
        {
            var currentUserId = this.CurrentUserSessionContext().UserId;
            Message message = _messageService.CreateSpeech(model.Mind, model.Speech, currentUserId);
            if (message != null)
            {
                _unitOfWork.Commit();
                TempData[ActionConfirmation.TempDataKey] = ActionConfirmation.CreateSuccess("Your speech has been created.");
                return RedirectToActionPermanent("Edit", new { @id = message.Id });
            }

            TempData[ActionConfirmation.TempDataKey] = ActionConfirmation.CreateError("Your speech has not been created. Try again.");
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var currentUserId = this.CurrentUserSessionContext().UserId;
            if(!_messageService.HasRightToMessage(currentUserId, id))
            {
                return new HttpAccessDeniedResult();
            }

            SpeechViewModel speechViewModel = _speechesViewModelService.CreateSpeechViewModel(id);
            return View(speechViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SpeechViewModel model)
        {
            var currentUserId = this.CurrentUserSessionContext().UserId;
            if (!_messageService.HasRightToMessage(currentUserId, model.MessageId))
            {
                return new HttpAccessDeniedResult();
            }

            Message message = _messageService.UpdateSpeech(model.MessageId, model.Mind, model.Speech);
            if (message != null)
            {
                _unitOfWork.Commit();
                TempData[ActionConfirmation.TempDataKey] = ActionConfirmation.CreateSuccess("Your speech has been saved.");
                return RedirectToActionPermanent("Edit", new { @id = message.Id });
            }

            TempData[ActionConfirmation.TempDataKey] = ActionConfirmation.CreateError("Your speech has not been saved. Try again.");
            return View(model);
        }
    }
}