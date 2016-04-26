using System.ComponentModel.DataAnnotations;

namespace WebUI.ViewModels.Manage
{
    public class PersonalInformationViewModel
    {
        [RegularExpression("[A-Za-z\\-]*", ErrorMessage = "Name should contain only alphabetic characters")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [RegularExpression("[A-Za-z\\-]*", ErrorMessage = "Last name should contain only alphabetic characters")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [RegularExpression("[A-Za-z\\-]*", ErrorMessage = "Middle name should contain only alphabetic characters")]
        [Display(Name = "Middle name")]
        public string MiddleName { get; set; }

        [Display(Name = "Phone")]
        [RegularExpression("[\\+]?[0-9]*", ErrorMessage = "Please enter only numeric symbols")]
        public string Phone { get; set; }

        
        [Display(Name = "Zip code")]
        [RegularExpression("[0-9]+", ErrorMessage = "Please enter only numeric symbols")]
        public string ZipCode { get; set; }

        [Display(Name = "Street")]
        public string Street { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Region")]
        public string Region { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Building number")]
        public string BuildingNumber { get; set; }

        public int Index { get; set; }
    }
}
