using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        private List<Appointment_User> Appointment_Users { get; set; }

        public IEnumerable<ApplicationUser> Users
        {
            get
            {
                return Appointment_Users.Select(a => a.User);
            }
        }
    }
}
