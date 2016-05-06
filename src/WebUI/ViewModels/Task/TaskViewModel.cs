using Domain.Entities;
using System;

namespace WebUI.ViewModels.Task
{
    public class TaskViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int? TaskId { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime Recurency { get; set; }
        public int Coins { get; set; }
        public DateTime? ExpireDt { get; set; }
        public Status Status { get; set; }

        public int TimeLeft
        {

            get
            {
                if (ExpireDt != null)
                    return ((DateTime)ExpireDt  - DateTime.Now).Days ;
                return 0;
            }
        }

        public TaskViewModel(){ }

        public TaskViewModel(ApplicationTask appTask)
        {
            UserName = "";
            Name = appTask.Name;
            Text = appTask.Text;
            Recurency = appTask.Recurency;
            Coins = Coins;
            TaskId = appTask.Id;
        }

        public TaskViewModel(UserTask userTask)
        {
            if (userTask.ApplicationTask != null)
            {
                Name = userTask.ApplicationTask.Name;
                
            }

            if (userTask.User != null) UserName = userTask.User.UserName;
            Text = userTask.Text;
            UserId = userTask.UserId;
            TaskId = userTask.TaskId;
            Status = userTask.Status;
            Coins = userTask.Coins;
            ExpireDt = userTask.ExpireDt;
        }
    }
}
