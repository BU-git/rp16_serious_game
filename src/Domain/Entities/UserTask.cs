using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserTask
    {
        public string UserId { get; set; }
        public int TaskId { get; set; }
        public Status Status { get; set; }
        public int Coins { get; set; }
        public DateTime ExpireDt { get; set; }

        public ApplicationUser User { get; set; }
        public ApplicationTask ApplicationTask { get; set; }
    }
    public enum Status { Open, Resolved,Completed,Reopened,Expired,Closed}
}
