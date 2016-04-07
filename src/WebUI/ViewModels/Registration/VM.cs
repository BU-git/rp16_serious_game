using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace WebUI.ViewModels.Registration
{
    //[Bind("Models", Prefix = "")]
    public class VM
    {
        [FromServices]
        public List<UserViewModel> Models { get; set; }

        public VM(List<UserViewModel> models)
        {
            Models = models;
        }

        public VM()
        {
            
        }
    }
}
