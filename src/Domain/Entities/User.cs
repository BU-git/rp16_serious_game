using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Gender { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MidleName { get; set; }
        public DateTime BirthDate { get; set; }

        public string Passport { get; set; }
        public int BSN { get; set; }

        public string ZipCode { get; set; }
        public string Street { get; set; }

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }

    }
}
