using System;
using System.Linq;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Core;

namespace SunLine.Community.Services.Core
{
    [BusinessLogic]
    public class ErrorService : IErrorService
    {
        private readonly IErrorRepository _errorRepository;
        private readonly IUserRepository _userRepository;

        public ErrorService(IErrorRepository errorRepository, IUserRepository userRepository)
        {
            _errorRepository = errorRepository;
            _userRepository = userRepository;
        }

        public Error Create(string errorMessage)
        {
            return Create(null, errorMessage);
        }

        public Error Create(Guid userId, string errorMessage)
        {
            User user = _userRepository.FindAll().FirstOrDefault(x => x.Id == userId);
            return Create(user, errorMessage);
        }

        private Error Create(User user, string errorMessage)
        {
            return _errorRepository.Create(new Error { User = user, ErrorMessage = errorMessage, Id = Guid.NewGuid(), CreationDate = DateTime.Now });
        }
    }
}
