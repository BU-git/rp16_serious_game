using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace WebUI.ViewModels.Task
{
    public class TaskListViewModel
    {
        public List<TaskViewModel> UserTasks { get; set; }

        public TaskListViewModel(){ }

        public TaskListViewModel(List<UserTask> userTasks )
        {
            UserTasks = new List<TaskViewModel>();
            foreach (var task in userTasks)
            {
                TaskViewModel t = new TaskViewModel(task);
                this.UserTasks.Add(t);
            }
        }
    }
}
