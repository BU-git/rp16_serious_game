using System.Collections.Generic;

namespace Domain.Entities
{
    public class Avatar
    {
        public Avatar()
        {
            this.ApplicationUser_Avatars = new List<ApplicationUser_Avatar>();
        }

        public int AvatarId { get; set; }
        public int Type { get; set; }
        public int Price { get; set; }

        public Media Media { get; set; }
        public List<ApplicationUser_Avatar> ApplicationUser_Avatars { get; set; }
    }
}
