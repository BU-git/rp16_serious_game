using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.ViewModels.Registration
{
    public class FamilyViewModel
    {
        public List<UserViewModel> Users { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "Family name")]
        public string FamilyName { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "Zip code")]
        [RegularExpression("[\\d]+", ErrorMessage = "Please enter only numeric symbols")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "Street")]
        public string Street { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "Region")]
        public string Region { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "Building number")]
        [RegularExpression("[\\d]+", ErrorMessage = "Please enter only positive numeric symbols")]
        public string BuildingNumber { get; set; }
    }
}
