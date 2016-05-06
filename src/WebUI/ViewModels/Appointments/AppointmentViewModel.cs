using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebUI.ViewModels.Appointments
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        public string Description { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        public DateTime End { get; set; }

        public List<ApplicationUser> Users { get; set; }
    }
}
