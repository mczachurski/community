using System.Data.Entity.Migrations;

namespace SunLine.Community.Repositories.Migrations
{
    public partial class ChangeNameOdRelationWithQuotedMessage : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Messages", name: "QutedMessage_Id", newName: "QuotedMessage_Id");
            RenameIndex(table: "dbo.Messages", name: "IX_QutedMessage_Id", newName: "IX_QuotedMessage_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Messages", name: "IX_QuotedMessage_Id", newName: "IX_QutedMessage_Id");
            RenameColumn(table: "dbo.Messages", name: "QuotedMessage_Id", newName: "QutedMessage_Id");
        }
    }
}
