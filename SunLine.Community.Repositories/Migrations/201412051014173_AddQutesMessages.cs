using System.Data.Entity.Migrations;

namespace SunLine.Community.Repositories.Migrations
{   
    public partial class AddQutesMessages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "QutedMessage_Id", c => c.Guid());
            CreateIndex("dbo.Messages", "QutedMessage_Id");
            AddForeignKey("dbo.Messages", "QutedMessage_Id", "dbo.Messages", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "QutedMessage_Id", "dbo.Messages");
            DropIndex("dbo.Messages", new[] { "QutedMessage_Id" });
            DropColumn("dbo.Messages", "QutedMessage_Id");
        }
    }
}
