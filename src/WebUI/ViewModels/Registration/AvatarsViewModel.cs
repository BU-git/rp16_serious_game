using System.Collections.Generic;
using Domain.Entities;

namespace WebUI.ViewModels.Registration
{
    public class AvatarsViewModel
    {
        public int AvatarId { get; set; }
        public List<Avatar> Avatars { get; set; }
    }
}
