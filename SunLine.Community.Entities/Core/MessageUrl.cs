using System;
using System.ComponentModel.DataAnnotations;

namespace SunLine.Community.Entities.Core
{
    public class MessageUrl : BaseEntity
    {
        [Required]
        public virtual Message Message { get; set; }

        [Required]
        public virtual String OriginalUrl { get; set; }

        [Required]
        public virtual String ShortenedlUrl { get; set; }

        [Required]
        public virtual int Index { get; set; }

        [Required]
        public virtual int Length { get; set; }
    }
}
