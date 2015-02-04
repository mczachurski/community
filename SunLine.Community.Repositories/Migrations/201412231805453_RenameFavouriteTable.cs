namespace SunLine.Community.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameFavouriteTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.FavouriteUserMessages", newName: "MessageFavourites");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.MessageFavourites", newName: "FavouriteUserMessages");
        }
    }
}
