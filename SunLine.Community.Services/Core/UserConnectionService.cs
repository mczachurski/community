using System;
using System.Collections.Generic;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Core;

namespace SunLine.Community.Services.Core
{
    [BusinessLogic]
    public class UserConnectionService : IUserConnectionService
    {
        private readonly IUserConnectionRepository _userConnectionRepository;
        private readonly IUserRepository _userRepository;

        public UserConnectionService(IUserConnectionRepository userConnectionRepository, IUserRepository userRepository)
        {
            _userConnectionRepository = userConnectionRepository;
            _userRepository = userRepository;
        }

        public bool IsConnectionBetweenUsers(Guid fromUserId, Guid toUserId)
        {
            return _userConnectionRepository.IsConnectionBetweenUsers(fromUserId, toUserId);
        }

        public bool ToggleConnection(Guid fromUserId, Guid toUserId)
        {
            if (fromUserId == toUserId)
            {
                return false;
            }

            var userConnection = _userConnectionRepository.FindUserConnection(fromUserId, toUserId);
            if (userConnection == null)
            {
                var fromUser = _userRepository.FindById(fromUserId);
                var toUser = _userRepository.FindById(toUserId);

                if (fromUser != null && toUser != null)
                {
                    userConnection = new UserConnection { FromUser = fromUser, ToUser = toUser };
                    _userConnectionRepository.Create(userConnection);
                    return true;
                }

                return false;
            }

            _userConnectionRepository.Delete(userConnection);
            return false;
        }

        public int AmountOfAllUserObservers(Guid userId)
        {
            return _userConnectionRepository.AmountOfAllUserObservers(userId);
        }

        public int AmountOfAllObservedByUser(Guid userId)
        {
            return _userConnectionRepository.AmountOfAllObservedByUser(userId);
        }
            
        public IList<User> FindUserObservers(Guid userId, int page, int amountOnPage)
        {
            return _userConnectionRepository.FindUserObservers(userId, page, amountOnPage);
        }

        public IList<User> FindObservedByUser(Guid userId, int page, int amountOnPage)
        {
            return _userConnectionRepository.FindObservedByUser(userId, page, amountOnPage);
        }
    }
}