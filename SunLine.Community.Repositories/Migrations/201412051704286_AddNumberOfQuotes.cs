using System.Data.Entity.Migrations;

namespace SunLine.Community.Repositories.Migrations
{    
    public partial class AddNumberOfQuotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "AmountOfQuotes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "AmountOfQuotes");
        }
    }
}
