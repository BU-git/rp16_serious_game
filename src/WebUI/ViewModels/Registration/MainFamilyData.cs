using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace WebUI.ViewModels.Registration
{
    public class MainFamilyData
    {
        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "Family name")]
        [RegularExpression("[a-zA-Z]+", ErrorMessage = "Family name should contain only alphabetic characters")]
        public string FamilyName { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Please, enter a valid email address (e.g. address@email.com)")]
        public string HeadEmail { get; set; }

        [Required(ErrorMessage = "Please select a family type")]
        [Display(Name = "Family type")]
        [RegularExpression("[a-zA-Z]+", ErrorMessage = "Please, choose a family type")]
        public UserGroupType FamilyType { get; set; }
    }
}
