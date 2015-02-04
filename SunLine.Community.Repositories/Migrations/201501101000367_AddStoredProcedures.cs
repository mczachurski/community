using System.Data.Entity.Migrations;
using System.IO;
using System.Reflection;

namespace SunLine.Community.Repositories.Migrations
{
    public partial class AddStoredProcedures : DbMigration
    {
        public override void Up()
        {
            string splitStringsFunction = EmbededResourceHelper.ReadFromEmbededResource("SunLine.Community.Repositories.Migrations.Functions.SplitStrings.sql");
            Sql(splitStringsFunction);

            string sendTransmitProcedure = EmbededResourceHelper.ReadFromEmbededResource("SunLine.Community.Repositories.Migrations.StoredProcedures.SendTransmit.sql");
            Sql(sendTransmitProcedure);

            string sendMentionsToUsersProcedure = EmbededResourceHelper.ReadFromEmbededResource("SunLine.Community.Repositories.Migrations.StoredProcedures.SendMentionsToUsers.sql");
            Sql(sendMentionsToUsersProcedure);

            string sendMessageToObserversProcedure = EmbededResourceHelper.ReadFromEmbededResource("SunLine.Community.Repositories.Migrations.StoredProcedures.SendMessageToObservers.sql");
            Sql(sendMessageToObserversProcedure);

            string publishMessageProcedure = EmbededResourceHelper.ReadFromEmbededResource("SunLine.Community.Repositories.Migrations.StoredProcedures.PublishMessage.sql");
            Sql(publishMessageProcedure);
        }
        
        public override void Down()
        {
        }
    }
}
