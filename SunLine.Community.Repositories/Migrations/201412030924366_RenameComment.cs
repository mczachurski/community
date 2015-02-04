namespace SunLine.Community.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameComment : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Messages", name: "Comment_Id", newName: "CommentedMessage_Id");
            RenameIndex(table: "dbo.Messages", name: "IX_Comment_Id", newName: "IX_CommentedMessage_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Messages", name: "IX_CommentedMessage_Id", newName: "IX_Comment_Id");
            RenameColumn(table: "dbo.Messages", name: "CommentedMessage_Id", newName: "Comment_Id");
        }
    }
}
