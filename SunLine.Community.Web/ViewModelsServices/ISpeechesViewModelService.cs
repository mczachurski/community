using SunLine.Community.Web.Common;
using SunLine.Community.Web.ViewModels.Messages;
using System;
using SunLine.Community.Entities.Core;
using SunLine.Community.Services.Core;
using System.Collections.Generic;

namespace SunLine.Community.Web.ViewModelsServices
{
    public interface ISpeechesViewModelService
    {
        IList<SpeechViewModel> CreateSpeechesViewModel(Guid userId);
        SpeechViewModel CreateSpeechViewModel(Guid? messageId = null);
    }
}
