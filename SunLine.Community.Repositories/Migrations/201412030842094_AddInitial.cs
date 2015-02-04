namespace SunLine.Community.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInitial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Version = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Mind = c.String(nullable: false, maxLength: 200),
                        Speech = c.String(),
                        AmountOfFavourites = c.Int(nullable: false),
                        AmountOfTransmitted = c.Int(nullable: false),
                        Version = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(),
                        Comment_Id = c.Guid(),
                        Language_Id = c.Guid(nullable: false),
                        MessageState_Id = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Messages", t => t.Comment_Id)
                .ForeignKey("dbo.Languages", t => t.Language_Id)
                .ForeignKey("dbo.MessageStates", t => t.MessageState_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Comment_Id)
                .Index(t => t.Language_Id)
                .Index(t => t.MessageState_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Path = c.String(nullable: false, maxLength: 500),
                        Identifier = c.Int(nullable: false),
                        Version = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(),
                        FileType_Id = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileTypes", t => t.FileType_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.FileType_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.FileTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        FileTypeEnum = c.Int(nullable: false),
                        Version = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Version = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(nullable: false, maxLength: 100),
                        GravatarHash = c.String(nullable: false, maxLength: 100),
                        Email = c.String(maxLength: 100),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Language_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Languages", t => t.Language_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Language_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Guid(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Code = c.String(nullable: false, maxLength: 5),
                        Version = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.UserCategories",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        AmountOfFavouritesToShow = c.Int(nullable: false),
                        Version = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(),
                        Category_Id = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Category_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.UserMessages",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        IsUserFavouriteMessage = c.Boolean(nullable: false),
                        IsMarkerSet = c.Boolean(nullable: false),
                        HaveMention = c.Boolean(nullable: false),
                        WasTransmitted = c.Boolean(nullable: false),
                        Version = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(),
                        Message_Id = c.Guid(nullable: false),
                        TransmittedUserMessage_Id = c.Guid(),
                        User_Id = c.Guid(nullable: false),
                        UserMessageCreationMode_Id = c.Guid(nullable: false),
                        UserMessageState_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Messages", t => t.Message_Id)
                .ForeignKey("dbo.UserMessages", t => t.TransmittedUserMessage_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.UserMessageCreationModes", t => t.UserMessageCreationMode_Id)
                .ForeignKey("dbo.UserMessageStates", t => t.UserMessageState_Id)
                .Index(t => t.Message_Id)
                .Index(t => t.TransmittedUserMessage_Id)
                .Index(t => t.User_Id)
                .Index(t => t.UserMessageCreationMode_Id)
                .Index(t => t.UserMessageState_Id);
            
            CreateTable(
                "dbo.UserMessageCreationModes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        UserMessageCreationModeEnum = c.Int(nullable: false),
                        Version = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserMessageStates",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        UserMessageStateEnum = c.Int(nullable: false),
                        Version = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MessageHashtags",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Version = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(),
                        Hashtag_Id = c.Guid(nullable: false),
                        Message_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hashtags", t => t.Hashtag_Id)
                .ForeignKey("dbo.Messages", t => t.Message_Id)
                .Index(t => t.Hashtag_Id)
                .Index(t => t.Message_Id);
            
            CreateTable(
                "dbo.Hashtags",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Version = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.MessageStates",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        MessageStateEnum = c.Int(nullable: false),
                        Version = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Errors",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ErrorMessage = c.String(),
                        Version = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(),
                        User_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Key = c.String(nullable: false),
                        Value = c.String(),
                        Version = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserConnections",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Version = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(),
                        FromUser_Id = c.Guid(nullable: false),
                        ToUser_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.FromUser_Id)
                .ForeignKey("dbo.Users", t => t.ToUser_Id)
                .Index(t => t.FromUser_Id)
                .Index(t => t.ToUser_Id);
            
            CreateTable(
                "dbo.MessageCategories",
                c => new
                    {
                        Message_Id = c.Guid(nullable: false),
                        Category_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Message_Id, t.Category_Id })
                .ForeignKey("dbo.Messages", t => t.Message_Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .Index(t => t.Message_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.FileMessages",
                c => new
                    {
                        File_Id = c.Guid(nullable: false),
                        Message_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.File_Id, t.Message_Id })
                .ForeignKey("dbo.Files", t => t.File_Id)
                .ForeignKey("dbo.Messages", t => t.Message_Id)
                .Index(t => t.File_Id)
                .Index(t => t.Message_Id);
            
            CreateTable(
                "dbo.UserLanguages",
                c => new
                    {
                        User_Id = c.Guid(nullable: false),
                        Language_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Language_Id })
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.Languages", t => t.Language_Id)
                .Index(t => t.User_Id)
                .Index(t => t.Language_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserConnections", "ToUser_Id", "dbo.Users");
            DropForeignKey("dbo.UserConnections", "FromUser_Id", "dbo.Users");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Errors", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Messages", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Messages", "MessageState_Id", "dbo.MessageStates");
            DropForeignKey("dbo.MessageHashtags", "Message_Id", "dbo.Messages");
            DropForeignKey("dbo.MessageHashtags", "Hashtag_Id", "dbo.Hashtags");
            DropForeignKey("dbo.Messages", "Language_Id", "dbo.Languages");
            DropForeignKey("dbo.Files", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserMessages", "UserMessageState_Id", "dbo.UserMessageStates");
            DropForeignKey("dbo.UserMessages", "UserMessageCreationMode_Id", "dbo.UserMessageCreationModes");
            DropForeignKey("dbo.UserMessages", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserMessages", "TransmittedUserMessage_Id", "dbo.UserMessages");
            DropForeignKey("dbo.UserMessages", "Message_Id", "dbo.Messages");
            DropForeignKey("dbo.UserLanguages", "Language_Id", "dbo.Languages");
            DropForeignKey("dbo.UserLanguages", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserCategories", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserCategories", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "Language_Id", "dbo.Languages");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.FileMessages", "Message_Id", "dbo.Messages");
            DropForeignKey("dbo.FileMessages", "File_Id", "dbo.Files");
            DropForeignKey("dbo.Files", "FileType_Id", "dbo.FileTypes");
            DropForeignKey("dbo.Messages", "Comment_Id", "dbo.Messages");
            DropForeignKey("dbo.MessageCategories", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.MessageCategories", "Message_Id", "dbo.Messages");
            DropIndex("dbo.UserLanguages", new[] { "Language_Id" });
            DropIndex("dbo.UserLanguages", new[] { "User_Id" });
            DropIndex("dbo.FileMessages", new[] { "Message_Id" });
            DropIndex("dbo.FileMessages", new[] { "File_Id" });
            DropIndex("dbo.MessageCategories", new[] { "Category_Id" });
            DropIndex("dbo.MessageCategories", new[] { "Message_Id" });
            DropIndex("dbo.UserConnections", new[] { "ToUser_Id" });
            DropIndex("dbo.UserConnections", new[] { "FromUser_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Errors", new[] { "User_Id" });
            DropIndex("dbo.Hashtags", new[] { "Name" });
            DropIndex("dbo.MessageHashtags", new[] { "Message_Id" });
            DropIndex("dbo.MessageHashtags", new[] { "Hashtag_Id" });
            DropIndex("dbo.UserMessages", new[] { "UserMessageState_Id" });
            DropIndex("dbo.UserMessages", new[] { "UserMessageCreationMode_Id" });
            DropIndex("dbo.UserMessages", new[] { "User_Id" });
            DropIndex("dbo.UserMessages", new[] { "TransmittedUserMessage_Id" });
            DropIndex("dbo.UserMessages", new[] { "Message_Id" });
            DropIndex("dbo.UserCategories", new[] { "User_Id" });
            DropIndex("dbo.UserCategories", new[] { "Category_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "Language_Id" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.Files", new[] { "User_Id" });
            DropIndex("dbo.Files", new[] { "FileType_Id" });
            DropIndex("dbo.Messages", new[] { "User_Id" });
            DropIndex("dbo.Messages", new[] { "MessageState_Id" });
            DropIndex("dbo.Messages", new[] { "Language_Id" });
            DropIndex("dbo.Messages", new[] { "Comment_Id" });
            DropTable("dbo.UserLanguages");
            DropTable("dbo.FileMessages");
            DropTable("dbo.MessageCategories");
            DropTable("dbo.UserConnections");
            DropTable("dbo.Settings");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Errors");
            DropTable("dbo.MessageStates");
            DropTable("dbo.Hashtags");
            DropTable("dbo.MessageHashtags");
            DropTable("dbo.UserMessageStates");
            DropTable("dbo.UserMessageCreationModes");
            DropTable("dbo.UserMessages");
            DropTable("dbo.UserCategories");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Languages");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.FileTypes");
            DropTable("dbo.Files");
            DropTable("dbo.Messages");
            DropTable("dbo.Categories");
        }
    }
}
