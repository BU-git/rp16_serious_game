using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using WebUI.ViewModels.FamRegistration;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: /<controller>/
        public IActionResult StepOne()
        {
            Step1RegistrationViewModel regVm = new Step1RegistrationViewModel();

            return View(regVm);
        }

        public PartialViewResult RegistrationForm()
        {
            return PartialView("_RegForm", new MainUserData());
        }
        
        [HttpPost]
        public PartialViewResult StepOne([FromBody]List<MainUserData> regVm)
        {
            foreach (MainUserData user in regVm)
            {
                if (!string.IsNullOrEmpty(user.Email))
                {
                    //TODO: Send message to the user with confirmation
                }
            }

            return PartialView("_Success", regVm);
        }
    }
}
