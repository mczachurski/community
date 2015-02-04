using System;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Core;

namespace SunLine.Community.Services.Core
{
    [BusinessLogic]
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User FindById(Guid id)
        {
            return _userRepository.FindById(id);
        }

        public User FindByUserName(string userName)
        {
            return _userRepository.FindByUserName(userName);
        }

        public void Update(User user)
        {
            _userRepository.Update(user);
        }
    }
}