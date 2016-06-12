using System.Collections.Generic;
using Domain.Entities;

namespace WebUI.ViewModels.Task
{
    public class AddTaskViewModel
    {
        public List<ApplicationUser> Users { get; set; }
        public List<ApplicationTask> Tasks { get; set; } 
    }
}
