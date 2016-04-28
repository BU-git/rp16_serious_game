using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.ViewModels.Email
{
    public class RegistrationMessage
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }
        public string LinkUrl { get; set; }
        public string Host { get; set; }
    }
}
