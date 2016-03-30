using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using RP16_SeriousGame.ViewModels.FamRegistration;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RP16_SeriousGame.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: /<controller>/
        public IActionResult StepOne()
        {
            return View();
        }

        [HttpPost]
        public void StepOne(Step1RegistrationViewModel regVm)
        {
            foreach (MainUserData data in regVm.UserData)
            {
                if (data.IsHead && !string.IsNullOrEmpty(data.Email))
                {
                    //TODO: Send message to the user with confirmation
                }
            }
        }
    }
}
