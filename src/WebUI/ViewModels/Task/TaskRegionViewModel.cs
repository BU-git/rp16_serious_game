using System.Collections.Generic;
using Domain.Entities;

namespace WebUI.ViewModels.Task
{
    public class TaskRegionViewModel
    {
        public Region Region { get; set; }
        public List<UserTask> Tasks { get; set; }

        public TaskRegionViewModel(Region region, List<UserTask> tasks)
        {
            Region = region;
            Tasks = tasks;
        }
    }
}
