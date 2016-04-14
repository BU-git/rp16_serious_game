using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace WebUI.ViewModels.Registration
{
    public class UserViewModel
    {
        public Gender Gender { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }

        public int Index { get; set; }
    }
}
