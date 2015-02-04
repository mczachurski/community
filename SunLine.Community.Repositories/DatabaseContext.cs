using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using SunLine.Community.Entities.Core;
using SunLine.Community.Entities.Dict;

namespace SunLine.Community.Repositories
{
    public class DatabaseContext : IdentityDbContext<User, ApplicationRole, Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IDatabaseContext
    {
        public DatabaseContext() : base("DefaultConnection")
        {
            Configuration.ValidateOnSaveEnabled = false;
        }

        public IDbSet<Message> Messages { get; set; }
        public IDbSet<Error> Errors { get; set; }
        public IDbSet<File> Files { get; set; }
        public IDbSet<UserConnection> UserConnections { get; set; }
        public IDbSet<UserMessage> UserMessages { get; set; }
        public IDbSet<Setting> Settings { get; set; }
        public IDbSet<MigrationHistory> MigrationHistories { get; set; }
        public IDbSet<FileType> FileTypes { get; set; }
        public IDbSet<Language> Languages { get; set; }
        public IDbSet<Category> Categories { get; set; }
        public IDbSet<UserMessageCreationMode> UserMessageCreationModes { get; set; }
        public IDbSet<UserMessageState> UserMessageStates { get; set; }
        public IDbSet<MessageState> MessageStates { get; set; }
        public IDbSet<Hashtag> Hashtags { get; set; }
        public IDbSet<MessageHashtag> MessageHashtags { get; set; }
        public IDbSet<UserCategory> UserCategories { get; set; }
        public IDbSet<MessageFavourite> MessageFavourites { get; set; }
        public IDbSet<MessageMention> MessageMentions { get; set; }
        public IDbSet<MessageUrl> MessageUrls { get; set; }
        public IDbSet<CategoryFavouriteLevel> CategoryFavouriteLevels { get; set; }

        public IDbSet<TEntity> GetDbSet<TEntity>() where TEntity : class
        {
            return Set<TEntity>();
        }

        public void SetModifiedEntityState(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        public bool IsDetachedentityState(object entity)
        {
            return Entry(entity).State == EntityState.Detached;
        }

        public bool IsNewEntity(object entity)
        {
            return Entry(entity).State == EntityState.Added;
        }

        public void Reload(object entity)
        {
            Entry(entity).Reload();
        }

        public virtual void Commit()
        {
            SaveChanges();
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            int result = Database.ExecuteSqlCommand(sql, parameters);
            return result;
        }

        public DbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().Property(p => p.Email).HasMaxLength(100);
            modelBuilder.Entity<User>().Property(p => p.UserName).HasMaxLength(50);

            modelBuilder.Entity<User>()
                .HasMany(t => t.UserMessageLanguages)
                .WithMany(t => t.UserMessageLanguages)
                .Map(m => m.ToTable("UserLanguages"));

            modelBuilder.Entity<Message>()
                .HasMany(t => t.Categories)
                .WithMany(t => t.Messages)
                .Map(m => m.ToTable("MessageCategories"));
                
            modelBuilder.Entity<File>()
                .HasRequired(t => t.User)
                .WithMany(t => t.Files);

            modelBuilder.Entity<Message>().HasOptional<Message>(s => s.CommentedMessage)
                .WithMany(s => s.Comments);

            modelBuilder.Entity<Message>().HasOptional<Message>(s => s.QuotedMessage)
                .WithMany(s => s.Quotes);

            modelBuilder.Entity<UserMessage>().HasOptional<UserMessage>(s => s.TransmittedUserMessage)
                .WithMany(s => s.TransmittedUserMessages);
        }
    }
}
