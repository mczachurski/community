using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Infrastructure;
using SunLine.Community.Resources.Titles;
using SunLine.Community.Services.Core;
using SunLine.Community.Web.Common;
using SunLine.Community.Web.Extensions;
using SunLine.Community.Web.SessionContext;
using SunLine.Community.Web.ViewModels.Messages;
using SunLine.Community.Web.ViewModelsServices;

namespace SunLine.Community.Web.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly IMessagesViewModelService _messagesViewModelService;
        private readonly IUserMessageService _userMessageService;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;
        private readonly IMessageFavouriteService _messageFavouriteService;
        private readonly IFileService _fileService;

        public MessagesController(
            IMessagesViewModelService messagesViewModelService,
            IUserService userService, 
            IUnitOfWork unitOfWork, 
            IUserMessageService userMessageService, 
            IMessageService messageService, 
            IMessageFavouriteService messageFavouriteService, 
            IFileService fileService)
        {
            _messagesViewModelService = messagesViewModelService;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _userMessageService = userMessageService;
            _messageService = messageService;
            _messageFavouriteService = messageFavouriteService;
            _fileService = fileService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAjax(CreateMindViewModel model)
        {
            var timelineUserId = this.CurrentUserSessionContext().UserId;
            Message message = _messageService.CreateMind(model.Mind, timelineUserId, model.Files);
            if (message != null)
            {
                _unitOfWork.Commit();
                _userMessageService.PublishMessage(message);

                _messageService.ReloadMessage(message);
                var watcherUserId = timelineUserId;
                MessageViewModel messageViewModel = _messagesViewModelService.CreateMessageViewModel(message, timelineUserId, watcherUserId, true);
                string mindView = this.RenderRazorViewToString("_UserMessageMindPartial", messageViewModel);
                return Json(new { @success = true, @view = mindView, messageId = message.Id });
            }

            return Json(new { @success = false, @error = SharedMessage.IncorrectData });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCommentAjax(CreateMindViewModel model, Guid commentedMessageId)
        {
            var timelineUserId = this.CurrentUserSessionContext().UserId;
            Message message = _messageService.CreateComment(model.Mind, timelineUserId, commentedMessageId);
            if (message != null)
            {
                _unitOfWork.Commit();
                _userMessageService.PublishMessage(message);

                _messageService.ReloadMessage(message);
                var watcherUserId = timelineUserId;
                MessageViewModel commentMessageViewModel = _messagesViewModelService.CreateMessageViewModel(message, timelineUserId, watcherUserId);

                Message orginalMessage = _messageService.FindById(commentedMessageId);
                _messageService.ReloadMessage(orginalMessage);
                MessageViewModel orginalMessageViewModel = _messagesViewModelService.CreateMessageViewModel(orginalMessage, timelineUserId, watcherUserId);

                string commentView = this.RenderRazorViewToString("_UserMessageCommentPartial", commentMessageViewModel);
                string commentedActionView = this.RenderRazorViewToString("_UserMessageActionsPartial", orginalMessageViewModel);

                return Json(new { @success = true, @commentedMessageId = commentedMessageId, @commentedActionView = commentedActionView, @commentView = commentView });
            }

            return Json(new { @success = false, @error = SharedMessage.IncorrectData });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuoteAjax(CreateMindViewModel model, Guid quotedMessageId)
        {
            var timelineUserId = this.CurrentUserSessionContext().UserId;
            Message message = _messageService.CreateQuote(model.Mind, timelineUserId, quotedMessageId);
            if (message != null)
            {
                _unitOfWork.Commit();
                _userMessageService.PublishMessage(message);

                _messageService.ReloadMessage(message);
                var watcherUserId = timelineUserId;
                MessageViewModel messageViewModel = _messagesViewModelService.CreateMessageViewModel(message, timelineUserId, watcherUserId, true);
                string mindView = this.RenderRazorViewToString("_UserMessageMindPartial", messageViewModel);
                return Json(new { @success = true, @view = mindView, messageId = message.Id });
            }

            return Json(new { @success = false, @error = SharedMessage.IncorrectData });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Transmit(Guid messageId)
        {
            _userMessageService.SendTransmit(this.CurrentUserSessionContext().UserId, messageId);

            var message = _messageService.FindById(messageId);
            var timelineUserId = this.CurrentUserSessionContext().UserId;
            var watcherUserId = timelineUserId;

            MessageViewModel messageViewModel = _messagesViewModelService.CreateMessageViewModel(message, timelineUserId, watcherUserId);
            string actionsView = this.RenderRazorViewToString("_UserMessageActionsPartial", messageViewModel);

            return Json(new { @success = true, @view = actionsView });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Favourite(Guid messageId)
        {
            var message = _messageService.FindById(messageId);
            if (message == null)
            {
                return Json(new { @success = false, @error = SharedMessage.IncorrectData });
            }

            _messageFavouriteService.ToggleFavourite(this.CurrentUserSessionContext().UserId, message);
            _unitOfWork.Commit();

            var timelineUserId = this.CurrentUserSessionContext().UserId;
            var watcherUserId = timelineUserId;
            MessageViewModel messageViewModel = _messagesViewModelService.CreateMessageViewModel(message, timelineUserId, watcherUserId);
            string actionsView = this.RenderRazorViewToString("_UserMessageActionsPartial", messageViewModel);
            return Json(new { @success = true, @view = actionsView });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FavouriteComment(Guid messageId)
        {
            var message = _messageService.FindById(messageId);
            if (message == null)
            {
                return Json(new { @success = false, @error = SharedMessage.IncorrectData });
            }

            var user = _userService.FindById(this.CurrentUserSessionContext().UserId);
            _messageFavouriteService.ToggleFavourite(user.Id, message);
            _unitOfWork.Commit();

            var timelineUserId = this.CurrentUserSessionContext().UserId;
            var watcherUserId = timelineUserId;
            MessageViewModel messageViewModel = _messagesViewModelService.CreateMessageViewModel(message, timelineUserId, watcherUserId);
            string actionsView = this.RenderRazorViewToString("_UserMessageCommentActionsPartial", messageViewModel);
            return Json(new { @success = true, @view = actionsView });
        }
            
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Bubble(Guid messageId)
        {
            var message = _messageService.FindById(messageId);
            if (message == null)
            {
                return Json(new { @success = false, @error = SharedMessage.IncorrectData });
            }

            _userMessageService.ToggleBubble(this.CurrentUserSessionContext().UserId, message);
            _unitOfWork.Commit();

            var timelineUserId = this.CurrentUserSessionContext().UserId;
            var watcherUserId = timelineUserId;
            MessageViewModel messageViewModel = _messagesViewModelService.CreateMessageViewModel(message, timelineUserId, watcherUserId);
            string actionsView = this.RenderRazorViewToString("_UserMessageActionsPartial", messageViewModel);
            return Json(new { @success = true, @view = actionsView });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid messageId)
        {
            if(!_messageService.HasRightToMessage(this.CurrentUserSessionContext().UserId, messageId))
            {
                return new HttpAccessDeniedResult();
            }

            _messageService.Delete(messageId);
            return Json(new { @success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Publish(Guid messageId)
        {
            if(!_messageService.HasRightToMessage(this.CurrentUserSessionContext().UserId, messageId))
            {
                return new HttpAccessDeniedResult();
            }

            Message message = _messageService.FindById(messageId);
            _userMessageService.PublishMessage(message);

            return Json(new { @success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveUploadedFile()
        {
            Guid userId = this.CurrentUserSessionContext().UserId;
            var fileIds = new List<Guid>();
            
            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                if (file == null || file.ContentLength <= 0)
                {
                    continue;
                }

                File savedFile = _fileService.Create(userId, file.FileName, file.ContentType, file.ContentLength, file.InputStream);
                _unitOfWork.Commit();

                fileIds.Add(savedFile.Id);
            }

            return Json(new { @success = true, @fileIds = fileIds });
        }
    }
}