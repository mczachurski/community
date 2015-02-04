using System.Data.Entity.Migrations;

namespace SunLine.Community.Repositories.Migrations
{
    public partial class ChangeFileEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "CoverFile_Id", c => c.Guid());
            AddColumn("dbo.Files", "ContentType", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Files", "ContentLength", c => c.Int(nullable: false));
            CreateIndex("dbo.Users", "CoverFile_Id");
            AddForeignKey("dbo.Users", "CoverFile_Id", "dbo.Files", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "CoverFile_Id", "dbo.Files");
            DropIndex("dbo.Users", new[] { "CoverFile_Id" });
            DropColumn("dbo.Files", "ContentLength");
            DropColumn("dbo.Files", "ContentType");
            DropColumn("dbo.Users", "CoverFile_Id");
        }
    }
}
