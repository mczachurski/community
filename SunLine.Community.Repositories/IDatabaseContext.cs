using System;
using System.Data.Entity;
using SunLine.Community.Entities.Core;
using SunLine.Community.Entities.Dict;

namespace SunLine.Community.Repositories
{
    public interface IDatabaseContext : IDisposable
    {
        IDbSet<User> Users { get; set; }
        IDbSet<Message> Messages { get; set; }
        IDbSet<Error> Errors { get; set; }
        IDbSet<File> Files { get; set; }
        IDbSet<UserConnection> UserConnections { get; set; }
        IDbSet<UserMessage> UserMessages { get; set; }
        IDbSet<Setting> Settings { get; set; }
        IDbSet<MigrationHistory> MigrationHistories { get; set; }
        IDbSet<FileType> FileTypes { get; set; }
        IDbSet<Language> Languages { get; set; }
        IDbSet<Category> Categories { get; set; }
        IDbSet<UserMessageCreationMode> UserMessageCreationModes { get; set; }
        IDbSet<UserMessageState> UserMessageStates { get; set; }
        IDbSet<MessageState> MessageStates { get; set; }
        IDbSet<Hashtag> Hashtags { get; set; }
        IDbSet<MessageHashtag> MessageHashtags { get; set; }
        IDbSet<UserCategory> UserCategories { get; set; }
        IDbSet<MessageFavourite> MessageFavourites { get; set; }
        IDbSet<MessageMention> MessageMentions { get; set; }
        IDbSet<MessageUrl> MessageUrls { get; set; }
        IDbSet<CategoryFavouriteLevel> CategoryFavouriteLevels { get; set; }

        IDbSet<TEntity> GetDbSet<TEntity>() where TEntity : class;
        void SetModifiedEntityState(object entity);
        bool IsDetachedentityState(object entity);
        bool IsNewEntity(object entity);
        void Reload(object entity);

        void Commit();
        DbContextTransaction BeginTransaction();
        int ExecuteSqlCommand(string sql, params object[] parameters);
    }
}
