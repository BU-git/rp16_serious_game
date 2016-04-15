using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace WebUI.ViewModels.Manage
{
    public class PersonalInformationViewModel
    {

        public string Name { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Phone { get; set; }

        public int Index { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string BuildingNumber { get; set; }
    }
}
