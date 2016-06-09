using System;
using System.Collections.Generic;
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

        public int Coins { get; set; } = 0;

        public int? CurrentAvatarId { get; set; }

        public List<ApplicationUser_UserGroup> ApplicationUser_UserGroups { get; set; }
        public List<Customer> Customer { get; set; }
        public List<UserTask> UserTasks { get; set; }
        public List<ApplicationUser_Avatar> ApplicationUser_Avatars { get; set; }
        public virtual ICollection<Appointment_User> User_Appointments { get; set; }
    }

    public enum Gender
    {
        Male, Female, Other, None, Unknown
    }
}
