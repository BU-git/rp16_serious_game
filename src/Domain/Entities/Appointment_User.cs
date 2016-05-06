﻿namespace Domain.Entities
{
    public class AppointmentUser
    {
        public int AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public bool IsOwner { get; set; }
    }
}
