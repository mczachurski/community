using SunLine.Community.Web.Common;
using SunLine.Community.Web.ViewModels.Messages;
using System;
using SunLine.Community.Entities.Core;
using SunLine.Community.Services.Core;
using System.Collections.Generic;

namespace SunLine.Community.Web.ViewModelsServices
{
    [ViewModelService]
    public class SpeechesViewModelService : ISpeechesViewModelService
    {
        private readonly IMessageService _messageService;

        public SpeechesViewModelService(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public IList<SpeechViewModel> CreateSpeechesViewModel(Guid userId)
        {
            IList<Message> spechees = _messageService.FindAllSpeeches(userId);
            var speechesViewModel = new List<SpeechViewModel>();

            foreach (var item in spechees)
            {
                SpeechViewModel speechViewModel = CreateSpeechViewModel(item);
                speechesViewModel.Add(speechViewModel);
            }

            return speechesViewModel;
        }

        public SpeechViewModel CreateSpeechViewModel(Guid? messageId = null)
        {
            if (messageId.HasValue)
            {
                Message message = _messageService.FindById(messageId.Value);
                if (message != null)
                {
                    return CreateSpeechViewModel(message);
                }
            }

            return new SpeechViewModel();
        }

        static SpeechViewModel CreateSpeechViewModel(Message message)
        {
            var speechViewModel = new SpeechViewModel
            {
                MessageId = message.Id,
                Mind = message.Mind,
                Speech = message.Speech,
                MessageStateName = message.MessageState.Name,
                MessageStateEnum = message.MessageState.MessageStateEnum
            };
            return speechViewModel;
        }
    }
}
