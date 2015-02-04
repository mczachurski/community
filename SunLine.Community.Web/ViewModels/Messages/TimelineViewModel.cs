using System.Collections.Generic;

namespace SunLine.Community.Web.ViewModels.Messages
{
    public class TimelineViewModel
    {
        public TimelineViewModel()
        {
            MessageViewModels = new List<MessageViewModel>();
        }

        public IList<MessageViewModel> MessageViewModels { get; set; } 
    }
}
