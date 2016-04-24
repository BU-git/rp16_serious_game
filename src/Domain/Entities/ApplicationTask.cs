using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class ApplicationTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime Recurency { get; set; }
        public int Coins { get; set; }

        public List<UserTask> UserTasks { get; set; } 
    }
}
