using System;
using System.Data.Entity.Migrations;

namespace SunLine.Community.Repositories.Migrations
{
    public partial class AddCategoryLevel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryFavouriteLevels",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        FavouriteLevel = c.Int(nullable: false),
                        Version = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.UserCategories", "CategoryFavouriteLevel_Id", c => c.Guid(nullable: false));
            CreateIndex("dbo.UserCategories", "CategoryFavouriteLevel_Id");
            AddForeignKey("dbo.UserCategories", "CategoryFavouriteLevel_Id", "dbo.CategoryFavouriteLevels", "Id");
            DropColumn("dbo.UserCategories", "AmountOfFavouritesToShow");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserCategories", "AmountOfFavouritesToShow", c => c.Int(nullable: false));
            DropForeignKey("dbo.UserCategories", "CategoryFavouriteLevel_Id", "dbo.CategoryFavouriteLevels");
            DropIndex("dbo.UserCategories", new[] { "CategoryFavouriteLevel_Id" });
            DropColumn("dbo.UserCategories", "CategoryFavouriteLevel_Id");
            DropTable("dbo.CategoryFavouriteLevels");
        }
    }
}
