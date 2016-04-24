using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Avatar
    {
        public int Type { get; set; }
        public int Level { get; set; }
        public int MediaId { get; set; }
        public int UserId { get; set; }

        public Media Media { get; set; }
        public ApplicationUser User { get; set; }
    }
}
