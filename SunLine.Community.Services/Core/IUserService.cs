using System;
using SunLine.Community.Entities.Core;

namespace SunLine.Community.Services.Core
{
    public interface IUserService
    {
        User FindById(Guid id);
        User FindByUserName(string userName);
        void Update(User user);
    }
}
