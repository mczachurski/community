using System;
using System.Linq;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Core
{
    public class MessageFavouriteRepository : EntityRepository<MessageFavourite>, IMessageFavouriteRepository
    {
        public MessageFavouriteRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }

        public bool IsUserFavouriteMessage(Guid userId, Guid messageId)
        {
            return FindAll().Any(x => x.Message.Id == messageId && x.User.Id == userId);
        }

        public MessageFavourite GetUserFavouriteMessage(Guid userId, Guid messageId)
        {
            return FindAll().FirstOrDefault(x => x.Message.Id == messageId && x.User.Id == userId);
        }
    }
}
