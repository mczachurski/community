using System.Data.Entity.Migrations;

namespace SunLine.Community.Repositories.Migrations
{    
    public partial class AddHaveMentionInComments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserMessages", "HaveMentionInComments", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserMessages", "HaveMentionInComments");
        }
    }
}
