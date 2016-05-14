using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace WebUI.ViewModels.Task
{
    public class AddTaskViewModel
    {
        public List<ApplicationUser> Users { get; set; }
        public List<ApplicationTask> Tasks { get; set; } 
    }
}
