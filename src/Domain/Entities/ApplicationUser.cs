using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {

        public override string Id { get; set; }
        public Gender Gender { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }

        public string Passport { get; set; }
        public int Bsn { get; set; }

        public string ZipCode { get; set; }
        public string Street { get; set; }

        public string Country { get; set; }
        public string Region { get; set; }

        public string City { get; set; }
        public string BuildingNumber { get; set; }

        public string Phone { get; set; }
        
        public List<ApplicationUserUserGroup> ApplicationUserUserGroups { get; set; }
        public List<Customer> Customer { get; set; }
        public List<UserTask> UserTasks { get; set; }
        public Avatar Avatar { get; set; }

    }

    public enum Gender
    {
       Male,Female,Other,None,Unknown
    }
}
