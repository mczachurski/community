using System.Data.Entity.Migrations;

namespace SunLine.Community.Repositories.Migrations
{
    public partial class MoveFavouriteMessageToNewTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FavouriteUserMessages",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Version = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(),
                        Message_Id = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Messages", t => t.Message_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Message_Id)
                .Index(t => t.User_Id);
            
            DropColumn("dbo.UserMessages", "IsUserFavouriteMessage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserMessages", "IsUserFavouriteMessage", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.FavouriteUserMessages", "User_Id", "dbo.Users");
            DropForeignKey("dbo.FavouriteUserMessages", "Message_Id", "dbo.Messages");
            DropIndex("dbo.FavouriteUserMessages", new[] { "User_Id" });
            DropIndex("dbo.FavouriteUserMessages", new[] { "Message_Id" });
            DropTable("dbo.FavouriteUserMessages");
        }
    }
}
