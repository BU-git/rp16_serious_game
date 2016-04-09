using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.ViewModels.Registration
{
    public class FamilyViewModel
    {
        public List<UserViewModel> Users { get; set; }

        public string FamilyName { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string BuildingNumber { get; set; }
    }
}
