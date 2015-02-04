using System;
using System.Collections.Generic;
using SunLine.Community.Entities.Core;

namespace SunLine.Community.Services.Core
{
    public interface IUserConnectionService
    {
        bool ToggleConnection(Guid fromUserId, Guid toUserId);
        bool IsConnectionBetweenUsers(Guid fromUserId, Guid toUserId);
        int AmountOfAllUserObservers(Guid userId);
        int AmountOfAllObservedByUser(Guid userId);
        IList<User> FindUserObservers(Guid userId, int page, int amountOnPage);
        IList<User> FindObservedByUser(Guid userId, int page, int amountOnPage);
    }
}
