using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.ViewModels.Registration
{
    public class UserViewModel
    {
        public Gender Gender { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MidleName { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public string Email { get; set; }
        public bool IsHead { get; set; }

        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string BuildingNumber { get; set; }
        public string Phone { get; set; }
    }

    public enum Gender
    {
        MALE, FEMALE, OTHER, NONE, UNKNOWN
    }
}
