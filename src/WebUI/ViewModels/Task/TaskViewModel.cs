using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication;

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
            this.UserName = "";
            this.Name = appTask.Name;
            this.Text = appTask.Text;
            this.Recurency = appTask.Recurency;
            this.Coins = Coins;
            this.TaskId = appTask.Id;
        }

        public TaskViewModel(UserTask userTask)
        {
            if (userTask.ApplicationTask != null)
            {
                this.Name = userTask.ApplicationTask.Name;
                
            }

            if (userTask.User != null) this.UserName = userTask.User.UserName;
            this.Text = userTask.Text;
            this.UserId = userTask.UserId;
            this.TaskId = userTask.TaskId;
            this.Status = userTask.Status;
            this.Coins = userTask.Coins;
            this.ExpireDt = userTask.ExpireDt;
        }
    }
}
