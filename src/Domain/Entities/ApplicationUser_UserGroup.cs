using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ApplicationUserUserGroup
    {
        public string Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int UserGoupId { get; set; }
        public UserGroup UserGroup { get; set; }
    }
}
