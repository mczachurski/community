using System;
using System.Data.Entity.Migrations;

namespace SunLine.Community.Repositories.Migrations
{
    public partial class AddAmountOfComments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "AmountOfComments", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "AmountOfComments");
        }
    }
}
