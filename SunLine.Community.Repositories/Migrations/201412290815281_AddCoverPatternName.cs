using System;
using System.Data.Entity.Migrations;

namespace SunLine.Community.Repositories.Migrations
{
    public partial class AddCoverPatternName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "CoverPatternName", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "CoverPatternName");
        }
    }
}
