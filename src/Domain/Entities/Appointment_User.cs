using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Appointment_User
    {
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public bool IsOwner { get; set; }
    }
}
