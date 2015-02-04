using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SunLine.Community.Web.ViewModels.Messages
{
    public class CreateMindViewModel
    {
        public CreateMindViewModel()
        {
            Files = new List<Guid>();
        }

        [Required]
        [StringLength(200)]
        [DisplayName("Mind")]
        public string Mind { get; set; }

        public IList<Guid> Files { get; set; }
    }
}

