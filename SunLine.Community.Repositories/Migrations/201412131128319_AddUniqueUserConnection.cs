namespace SunLine.Community.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniqueUserConnection : DbMigration
    {
        public override void Up()
        {
            CreateIndex("UserConnections", new[] { "FromUser_Id", "ToUser_Id" }, true, "IX_UniqueConnection");
        }

        public override void Down()
        {
            DropIndex("UserConnections", "IX_UniqueConnection");
        }
    }
}
