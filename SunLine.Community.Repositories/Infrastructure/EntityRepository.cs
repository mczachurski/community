using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using SunLine.Community.Entities;

namespace SunLine.Community.Repositories.Infrastructure
{
    public class EntityRepository<T> where T : BaseEntity
    {
        protected IDbSession DbSession { get; private set; }
        protected IDbSet<T> DbSet { get; private set; }

        public EntityRepository(IDbSession dbSession)
        {
            DbSession = dbSession;
            DbSet = DbSession.Current.GetDbSet<T>();
        }
            
        public T Create(T entity)
        {
            entity.Version = 1;
            entity.CreationDate = DateTime.UtcNow;

            return DbSet.Add(entity);
        }
            
        public void Update(T entity)
        {
            if(DbSession.Current.IsNewEntity(entity))
            {
                return;
            }

            entity.Version++;
            entity.ModificationDate = DateTime.UtcNow;

            DbSet.Attach(entity);
            DbSession.Current.SetModifiedEntityState(entity);
        }
            
        public void Delete(T entity)
        {
            if (DbSession.Current.IsDetachedentityState(entity))
            {
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);
        }
            
        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }
            
        public T FindById(Guid id)
        {
            return DbSet.FirstOrDefault(o => o.Id == id);
        }
            
        public IQueryable<T> FindAll()
        {
            return DbSet;
        }

        public void ReloadEntity(T entity)
        {
            DbSession.Current.Reload(entity);
        }
    }
}
