namespace SunLine.Community.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniqueUserMessage : DbMigration
    {
        public override void Up()
        {
            CreateIndex("UserMessages", new[] { "Message_Id", "User_Id" }, true, "IX_UniqueMessage");
        }

        public override void Down()
        {
            DropIndex("UserMessages", "IX_UniqueMessage");
        }
    }
}
