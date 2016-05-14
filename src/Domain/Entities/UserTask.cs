using System;

namespace Domain.Entities
{
    public class UserTask
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TaskId { get; set; }
        public Status Status { get; set; }
        public int Coins { get; set; }
        public DateTime ExpireDt { get; set; }
        public string Text { get; set; }
        public DateTime? ResolutionDate { get; set; }

        public ApplicationUser User { get; set; }
        public ApplicationTask ApplicationTask { get; set; }
    }
    public enum Status { Open, Resolved, Completed, Reopened, Expired, Closed }
}
