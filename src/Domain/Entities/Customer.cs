using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime Birthplace { get; set; }
        public int Contact { get; set; }
        public int isActive { get; set; }
        public int Resident { get; set; }
        public DateTime ActivationDate { get; set; }
        public DateTime DisactivationDate { get; set; }
        public int RegisteredBy { get; set; }
        public string SchoolGrade { get; set; }

        public string Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
