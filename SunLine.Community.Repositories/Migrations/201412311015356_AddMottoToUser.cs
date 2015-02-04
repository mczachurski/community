namespace SunLine.Community.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMottoToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Motto", c => c.String(maxLength: 200));
            AddColumn("dbo.Users", "Location", c => c.String(maxLength: 100));
            AddColumn("dbo.Users", "WebAddress", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "WebAddress");
            DropColumn("dbo.Users", "Location");
            DropColumn("dbo.Users", "Motto");
        }
    }
}
