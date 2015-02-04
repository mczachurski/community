namespace SunLine.Community.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndexToUserMessage : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.UserMessages", "SortingDate");
            CreateIndex("dbo.UserMessages", new[] { "User_Id", "UserMessageState_Id" }, false, "IX_MessagesForUser");
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserMessages", "IX_MessagesForUser");
            DropIndex("dbo.UserMessages", new[] { "SortingDate" });
        }
    }
}
