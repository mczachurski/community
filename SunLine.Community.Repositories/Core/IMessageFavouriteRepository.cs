using System;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Core
{
    public interface IMessageFavouriteRepository : IEntityRepository<MessageFavourite>
    {
        bool IsUserFavouriteMessage(Guid userId, Guid messageId);
        MessageFavourite GetUserFavouriteMessage(Guid userId, Guid messageId);
    }
}
