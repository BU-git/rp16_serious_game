using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
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
        public string Name { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "Middle name")]
        public string MiddleName { get; set; }


        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "Year")]
        [Range(1900, 2020, ErrorMessage = "Please enter a valid year")]
        public int Year { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "Month")]
        [Range(1, 12, ErrorMessage = "The value should be between 1 and 12")]
        public int Month { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "Day")]
        [Range(1, 31, ErrorMessage = "The value should be between 1 and 31")]
        public int Day { get; set; }


        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Please, enter a valid email address (e.g. address@email.com)")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "Phone number")]
        [RegularExpression("[0-9]+", ErrorMessage = "Please enter only numeric symbols")]
        public string Phone { get; set; }

        public int Index { get; set; }
    }
}
