namespace SunLine.Community.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSortingDateToUserMessage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserMessages", "SortingDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserMessages", "UpdateSortingDateOnNewComment", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Messages", "CreationDate");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Messages", new[] { "CreationDate" });
            DropColumn("dbo.UserMessages", "UpdateSortingDateOnNewComment");
            DropColumn("dbo.UserMessages", "SortingDate");
        }
    }
}
