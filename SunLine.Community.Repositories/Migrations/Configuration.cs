using System.Data.Entity.Migrations;
using SunLine.Community.Repositories.Migrations.TableData.Core;
using SunLine.Community.Repositories.Migrations.TableData.Dict;

namespace SunLine.Community.Repositories.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(DatabaseContext context)
        {
            SettingTable.Initialize(context);
            FileTypeTable.Initialize(context);
            LanguageTable.Initialize(context);
            CategoryTable.Initialize(context);
            UserMessageCreationModeTable.Initialize(context);
            UserMessageStateTable.Initialize(context);
            MessageStateTable.Initialize(context);
            CategoryFavouriteLevelTable.Initialize(context);
        }
    }
}
