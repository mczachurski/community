using System;
using System.Linq;
using System.Linq.Expressions;
using SunLine.Community.Entities.Core;

namespace SunLine.Community.Repositories.Core
{
    public interface IUserRepository
    {
        User Create(User entity);
        void Update(User entity);
        void Delete(User entity);
        IQueryable<User> Find(Expression<Func<User, bool>> predicate);
        User FindById(Guid id);
        User FindByUserName(string userName);
        IQueryable<User> FindAll();
    }
}
