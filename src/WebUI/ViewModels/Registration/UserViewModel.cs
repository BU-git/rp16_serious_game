using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace WebUI.ViewModels.Registration
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "Gender")]
        [RegularExpression("[a-zA-Z]+", ErrorMessage = "Please, choose gender")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "Name")]
        [RegularExpression("[A-Za-z\\-]+", ErrorMessage = "Name should contain only alphabetic characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "Last name")]
        [RegularExpression("[A-Za-z\\-]+", ErrorMessage = "Last name should contain only alphabetic characters")]
        public string LastName { get; set; }

        [Display(Name = "Middle name")]
        [RegularExpression("[A-Za-z\\-]*", ErrorMessage = "Middle name should contain only alphabetic characters")]
        public string MiddleName { get; set; }


        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "Year")]
        [Range(1900, 2020, ErrorMessage = "Please enter a valid year")]
        [RegularExpression("[0-9]+", ErrorMessage = "Year should contain only numeric characters")]
        public int Year { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "Month")]
        [Range(1, 12, ErrorMessage = "The value should be between 1 and 12")]
        [RegularExpression("[0-9]+", ErrorMessage = "Month should contain only numeric characters")]
        public int Month { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "Day")]
        [Range(1, 31, ErrorMessage = "The value should be between 1 and 31")]
        [RegularExpression("[0-9]+", ErrorMessage = "Day should contain only numeric characters")]
        public int Day { get; set; }


        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Please, enter a valid email address (e.g. address@email.com)")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "Phone number")]
        [RegularExpression("[\\+]?[0-9]+", ErrorMessage = "Please enter only numeric symbols")]
        public string Phone { get; set; }

        public int Index { get; set; }
    }
}
