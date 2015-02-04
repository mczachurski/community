using System;
using System.Data.Entity;
using SunLine.Community.Repositories;
using SunLine.Community.Repositories.Migrations;

namespace SunLine.Community.Services
{
    public static class DatabaseSetup
    {
        public static void Init(bool forceInitialize = false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Configuration>());
            try
            {
                using (var context = new DatabaseContext())
                {
                    context.Database.Initialize(forceInitialize);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Initialization the database failed.", ex);
            }
        }
    }
}
