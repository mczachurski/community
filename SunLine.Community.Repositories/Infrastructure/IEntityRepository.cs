using System;
using System.Linq;
using System.Linq.Expressions;
using SunLine.Community.Entities;

namespace SunLine.Community.Repositories.Infrastructure
{
    public interface IEntityRepository<T> where T : BaseEntity
    {
        T Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindAll();
        T FindById(Guid id);
        void ReloadEntity(T entity);
    }
}
