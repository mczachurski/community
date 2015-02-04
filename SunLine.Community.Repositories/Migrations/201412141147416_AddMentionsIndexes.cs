namespace SunLine.Community.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMentionsIndexes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MessageMentions",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        Length = c.Int(nullable: false),
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
            
            CreateTable(
                "dbo.MessageUrls",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        OriginalUrl = c.String(nullable: false),
                        ShortenedlUrl = c.String(nullable: false),
                        Index = c.Int(nullable: false),
                        Length = c.Int(nullable: false),
                        Version = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(),
                        Message_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Messages", t => t.Message_Id)
                .Index(t => t.Message_Id);
            
            AddColumn("dbo.MessageHashtags", "Index", c => c.Int(nullable: false));
            AddColumn("dbo.MessageHashtags", "Length", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MessageUrls", "Message_Id", "dbo.Messages");
            DropForeignKey("dbo.MessageMentions", "User_Id", "dbo.Users");
            DropForeignKey("dbo.MessageMentions", "Message_Id", "dbo.Messages");
            DropIndex("dbo.MessageUrls", new[] { "Message_Id" });
            DropIndex("dbo.MessageMentions", new[] { "User_Id" });
            DropIndex("dbo.MessageMentions", new[] { "Message_Id" });
            DropColumn("dbo.MessageHashtags", "Length");
            DropColumn("dbo.MessageHashtags", "Index");
            DropTable("dbo.MessageUrls");
            DropTable("dbo.MessageMentions");
        }
    }
}
