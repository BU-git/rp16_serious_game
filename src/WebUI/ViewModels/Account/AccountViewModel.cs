using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace WebUI.ViewModels.Account
{
    public class AccountViewModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Coins { get; set; }
        public int? CurrentAvatarId { get; set; }

        public AccountViewModel()
        {
            
        }

        public AccountViewModel(ApplicationUser user)
        {
            UserId = user.Id;
            Name = user.Name;
            LastName = user.LastName;
            Coins = user.Coins;
            CurrentAvatarId = user.CurrentAvatarId;
        }
    }
}
