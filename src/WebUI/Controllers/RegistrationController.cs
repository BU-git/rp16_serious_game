using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BLL.Abstract;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using NUnit.Framework.Constraints;
using RP16_SeriousGame.Models;
using WebUI.Infrastructure.Abstract;
using WebUI.ViewModels.Registration;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IMailManager _mailManager;
        private readonly ICryptoServices _cryptoServices;

        public RegistrationController(IMailManager mailManager, ICryptoServices crypto)
        {
            _mailManager = mailManager;
            _cryptoServices = crypto;
        }

        public IActionResult StepOne()
        {
            return View(new MainFamilyData());
        }

        [HttpPost]
        public async Task<IActionResult> StepOne([FromForm]MainFamilyData regVm)
        {
            if (!ModelState.IsValid)
            {
                return View(regVm);
            }

            string randomPass = _cryptoServices.GenerateRandomPassword();

            //TODO: add user to DAL

            await _mailManager.SendRegistrationMailAsync(randomPass, regVm.HeadEmail);

            //TODO: add success message

            return View(new MainFamilyData());
        }

        public async Task<IActionResult> StepTwo()
        {
            //TODO: get main data 'bout family 

            var familyInfo = new FamilyViewModel
            {
                Users = new List<UserViewModel>
                {
                    new UserViewModel
                    {
                        //The user from DB
                    }
                }
            };

            return View(familyInfo);
        }

        [HttpPost]
        public async Task<IActionResult> StepTwo([FromForm]FamilyViewModel regVm)
        {
            //TODO: Put data into DB async

            return View(regVm);
        }

        [HttpPost]
        public PartialViewResult RegistrationForm(int index)
        {
            return PartialView("_StepTwoForm", new UserViewModel { Index = index });
        }
    }
}
