using System;
using System.Collections.Generic;
using System.Linq;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Core
{
    public class UserConnectionRepository : EntityRepository<UserConnection>, IUserConnectionRepository
    {
        public UserConnectionRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }

        public UserConnection FindUserConnection(Guid fromUserId, Guid toUserId)
        {
            return FindAll().FirstOrDefault(x => x.FromUser.Id == fromUserId && x.ToUser.Id == toUserId);
        }

        public bool IsConnectionBetweenUsers(Guid fromUserId, Guid toUserId)
        {
            return FindAll().Any(x => x.FromUser.Id == fromUserId && x.ToUser.Id == toUserId);
        }

        public IList<User> FindAllUserObservers(Guid userId)
        {
            return FindAll().Where(x => x.ToUser.Id == userId).Select(x => x.FromUser).ToList();
        }

        public IList<User> FindAllObservedByUser(Guid userId)
        {
            return FindAll().Where(x => x.FromUser.Id == userId).Select(x => x.ToUser).ToList();
        }

        public int AmountOfAllUserObservers(Guid userId)
        {
            return FindAll().Count(x => x.ToUser.Id == userId);
        }

        public int AmountOfAllObservedByUser(Guid userId)
        {
            return FindAll().Count(x => x.FromUser.Id == userId);
        }

        public IList<User> FindUserObservers(Guid userId, int page, int amountOnPage)
        {
            return FindAll().Where(x => x.ToUser.Id == userId).OrderByDescending(x => x.CreationDate)
                .Skip(page * amountOnPage).Take(amountOnPage).Select(x => x.FromUser).ToList();
        }

        public IList<User> FindObservedByUser(Guid userId, int page, int amountOnPage)
        {
            return FindAll().Where(x => x.FromUser.Id == userId).OrderByDescending(x => x.CreationDate)
                .Skip(page * amountOnPage).Take(amountOnPage).Select(x => x.ToUser).ToList();
        }
    }
}