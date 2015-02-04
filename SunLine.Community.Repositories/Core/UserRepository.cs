using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Core
{
    public class UserRepository : IUserRepository
    {
        protected IDbSession DbSession { get; private set; }
        protected IDbSet<User> DbSet { get; private set; }

        public UserRepository(IDbSession dbSession)
        {
            DbSession = dbSession;
            DbSet = DbSession.Current.GetDbSet<User>();
        }

        public User Create(User entity)
        {
            return DbSet.Add(entity);
        }

        public void Update(User entity)
        {
            DbSet.Attach(entity);
            DbSession.Current.SetModifiedEntityState(entity);
        }

        public void Delete(User entity)
        {
            if (DbSession.Current.IsDetachedentityState(entity))
            {
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);
        }

        public IQueryable<User> Find(Expression<Func<User, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public User FindById(Guid id)
        {
            return DbSet.FirstOrDefault(o => o.Id == id);
        }

        public User FindByUserName(string userName)
        {
            return DbSet.FirstOrDefault(o => o.UserName == userName);
        }

        public IQueryable<User> FindAll()
        {
            return DbSet;
        }
    }
}