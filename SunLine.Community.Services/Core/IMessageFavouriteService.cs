using System;
using SunLine.Community.Entities.Core;

namespace SunLine.Community.Services.Core
{
    public interface IMessageFavouriteService
    {
        void ToggleFavourite(Guid userId, Message message);
        bool IsUserFavouriteMessage(Guid userId, Guid messageId);
    }
}
