using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class UserTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public int TaskId { get; set; }
        public Status Status { get; set; }
        public int Coins { get; set; }
        public DateTime ExpireDt { get; set; }
        public string Text { get; set; }
        public DateTime? ResolutionDate { get; set; }

        public Region Region { get; set; }
        public string Country { get; set; }

        public ApplicationUser User { get; set; }
        public ApplicationTask ApplicationTask { get; set; }

        public List<Task_Comment> Task_Comments { get; set; }
    }
    public enum Status { Open, Resolved, Completed, Reopened, Expired, Closed }
    public enum Region { NorthAmerica, SouthAmerica, Africa, Europe, Australia, NorthAsia, NearEast, SouthAsia }
}
