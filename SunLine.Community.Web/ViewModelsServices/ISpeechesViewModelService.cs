using SunLine.Community.Web.ViewModels.Messages;
using System;
using System.Collections.Generic;

namespace SunLine.Community.Web.ViewModelsServices
{
    public interface ISpeechesViewModelService
    {
        IList<SpeechViewModel> CreateSpeechesViewModel(Guid userId);
        SpeechViewModel CreateSpeechViewModel(Guid? messageId = null);
    }
}
