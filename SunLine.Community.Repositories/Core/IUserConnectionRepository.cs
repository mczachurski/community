using System;
using System.Collections.Generic;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Core
{
    public interface IUserConnectionRepository : IEntityRepository<UserConnection>
    {
        bool IsConnectionBetweenUsers(Guid fromUserId, Guid toUserId);
        UserConnection FindUserConnection(Guid fromUserId, Guid toUserId);
        int AmountOfAllUserObservers(Guid userId);
        int AmountOfAllObservedByUser(Guid userId);
        IList<User> FindAllUserObservers(Guid userId);
        IList<User> FindAllObservedByUser(Guid userId);
        IList<User> FindUserObservers(Guid userId, int page, int amountOnPage);
        IList<User> FindObservedByUser(Guid userId, int page, int amountOnPage);
    }
}