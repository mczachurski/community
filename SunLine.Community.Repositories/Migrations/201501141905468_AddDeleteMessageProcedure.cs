using System.Data.Entity.Migrations;

namespace SunLine.Community.Repositories.Migrations
{
    public partial class AddDeleteMessageProcedure : DbMigration
    {
        public override void Up()
        {
            string deleteMessageProcedure = EmbededResourceHelper.ReadFromEmbededResource("SunLine.Community.Repositories.Migrations.StoredProcedures.DeleteMessage.sql");
            Sql(deleteMessageProcedure);
        }
        
        public override void Down()
        {
        }
    }
}
