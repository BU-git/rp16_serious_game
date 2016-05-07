using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public virtual ICollection<Appointment_User> Appointment_Users { get; set; }
    }
}
