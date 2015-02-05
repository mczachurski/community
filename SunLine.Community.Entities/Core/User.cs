using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SunLine.Community.Entities.Dict;

namespace SunLine.Community.Entities.Core
{
    public class User : IdentityUser<Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IBaseEntity
    {
        public User()
        {
            Id = Guid.NewGuid();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, Guid> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            return userIdentity;
        }

        [DefaultValue(1)]
        public virtual int Version { get; set; }

        [Required]
        public virtual DateTime CreationDate { get; set; }

        public virtual DateTime? ModificationDate { get; set; }

        [Required]
        [StringLength(100)]
        public virtual string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public virtual string LastName { get; set; }

        [Required]
        public virtual Language Language { get; set; }

        public virtual IList<Language> UserMessageLanguages { get; set; }

        public virtual IList<UserCategory> UserCategories { get; set; }

        public virtual IList<Message> Messages { get; set; }

        public virtual IList<UserMessage> UserMessages { get; set; }

        public virtual IList<File> Files { get; set; }

        public virtual IList<MessageFavourite> FavouriteMessages { get; set; }

        [Required]
        [StringLength(100)]
        public virtual string GravatarHash { get; set; }

        [StringLength(50)]
        public virtual string CoverPatternName { get; set; }

        public virtual File CoverFile { get; set; }

        [StringLength(200)]
        public virtual string Motto { get; set; }

        [StringLength(100)]
        public virtual string Location { get; set; }

        [StringLength(100)]
        public virtual string WebAddress { get; set; }

        public virtual string FullName
        {
            get { return String.Format("{0} {1}", FirstName, LastName); }
        }
    }

    public class ApplicationRole : IdentityRole<Guid, ApplicationUserRole>
    {
        public ApplicationRole()
        {
        }

        public ApplicationRole(string name)
        {
            Name = name;
        }
    }

    public class ApplicationUserLogin : IdentityUserLogin<Guid>
    {

    }

    public class ApplicationUserRole : IdentityUserRole<Guid>
    {

    }

    public class ApplicationUserClaim : IdentityUserClaim<Guid>
    {

    }
}