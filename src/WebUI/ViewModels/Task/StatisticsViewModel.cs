using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace WebUI.ViewModels.Task
{
    public class StatisticsViewModel
    {
        public List<GroupViewModel> GroupsTasks { get; set; }

        public StatisticsViewModel()
        {
            
        }

        public StatisticsViewModel(List<GroupViewModel> groupsTasks)
        {
            GroupsTasks = groupsTasks;
        }
    }

    public class GroupViewModel
    {
        public UserGroup UserGroup { get; set; }
        public List<UserTask> UserTasks { get; set; }
        public List<UserWithStatsViewModel> UserStats { get; set; }
    }

    public class UserWithStatsViewModel
    {
        public List<UserTask> Tasks { get; set; }
        public List<DailyStatistics> TasksByDays { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public int CompletedTasks { get; set; }
        public int TasksLeft { get; set; }

        public UserWithStatsViewModel(ApplicationUser user)
        {
            Name = user.Name;
            UserName = user.UserName;
        }
    }

    public class DailyStatistics
    {
        public DailyStatistics()
        {
            
        }

        public DailyStatistics(int tasksCompleted, int? date)
        {
            TasksCompleted = tasksCompleted;
            Day = date ?? default(int);
        }

        public int TasksCompleted { get; set; }
        public int Day { get; set; }
    }
}
