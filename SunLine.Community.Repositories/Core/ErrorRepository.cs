using System;
using System.Linq;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.Repositories.Core
{
    public class ErrorRepository : EntityRepository<Error>, IErrorRepository
    {
        public ErrorRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }

        /// <summary>
        /// Overloaded method. It must save changes to the database in a completely separate context. 
        /// This is done so because, as the saving was for the general context it was written data, 
        /// that should not be. For example after exception was executed Commit with incorrect data.
        /// </summary>
        /// <returns></returns>
        public new Error Create(Error error)
        {
            using (var databaseContext = new DatabaseContext())
            {
                error = databaseContext.Errors.Add(new Error
                    {
                        ErrorMessage = error.ErrorMessage, 
                        User = error.User != null ? databaseContext.Users.FirstOrDefault(x => x.Id == error.User.Id) : null,
                        CreationDate = DateTime.UtcNow,
                        Version = 1
                    });

                databaseContext.Commit();
            }

            return error;
        }
    }
}
