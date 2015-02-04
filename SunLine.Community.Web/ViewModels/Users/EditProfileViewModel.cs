using System;
using System.Collections.Generic;
using SunLine.Community.Entities.Dict;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SunLine.Community.Web.ViewModels.Users
{
    public class EditProfileViewModel
    {
        public EditProfileViewModel()
        {
            CategoryViewModels = new List<CategoryViewModel>();
            UserMessageLanguages = new List<DictViewModel<Guid>>();
            Categories = new List<Category>();
            Languages = new List<Language>();
            CoverPatternNames = new List<DictViewModel<string>>();
            CoverPatternName = string.Empty;
        }

        [Required]
        [StringLength(100)]
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Last name")]
        public string LastName { get; set; }

        [StringLength(200)]
        [DisplayName("Motto")]
        public string Motto { get; set; }

        [StringLength(100)]
        [DisplayName("Location")]
        public string Location { get; set; }

        [StringLength(100)]
        [DisplayName("Web Address")]
        public string WebAddress { get; set; }

        [Required]
        [DisplayName("Language")]
        public Guid Language { get; set; }

        [DisplayName("Profile cover pattern")]
        public string CoverPatternName { get ; set; }

        [DisplayName("Cover image")]
        public string CoverImageUrl { get; set; }

        [DisplayName("Preferred languages")]
        public IList<DictViewModel<Guid>> UserMessageLanguages { get; set; }

        [DisplayName("Preferred categories")]
        public IList<CategoryViewModel> CategoryViewModels { get; set; }

        public IList<Language> Languages { get; set; }
        public IList<Category> Categories { get; set; }
        public IList<DictViewModel<string>> CoverPatternNames { get ; set; } 
    }
}

