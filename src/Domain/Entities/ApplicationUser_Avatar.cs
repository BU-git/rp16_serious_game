using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ApplicationUser_Avatar
    {
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int AvatarId { get; set; }
        public Avatar Avatar { get; set; }
    }
}
