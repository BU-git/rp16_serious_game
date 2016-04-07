﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BLL.Abstract;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using NUnit.Framework.Constraints;
using WebUI.Infrastructure.Abstract;
using WebUI.ViewModels.FamRegistration;
using WebUI.ViewModels.Registration;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IMailManager _mailManager;

        public RegistrationController(IMailManager mailManager)
        {
            _mailManager = mailManager;
        }

        // GET: /<controller>/
        public IActionResult StepOne()
        {
            return View();
        }

        public PartialViewResult RegistrationForm()
        {
            return PartialView("_RegForm", new MainUserData());
        }
        
        [HttpPost]
        public async Task<PartialViewResult> StepOne([FromBody]List<MainUserData> regVm)
        {
            if (!ModelState.IsValid)
            {
                //TODO: let this return the whole page with data filled in..
                return null;
            }


            foreach (MainUserData user in regVm)
            {
                if (!string.IsNullOrEmpty(user.Email))
                {
                    //TODO: Send message to the user with confirmation
                }
            }

            await _mailManager.SendRegistrationMailAsync(Guid.NewGuid(), "egor.chankoff@gmail.com");

            return PartialView("_Success", regVm);
        }

        public async Task<IActionResult> StepTwo(Guid? id)
        {
            //TODO: Get users from DB
            if (!id.HasValue)
            {
                //return null;
            }

            List<UserViewModel> data = new List<UserViewModel> //mock
            {
                new UserViewModel { Name = "Jon", LastName = "Doe", IsHead = true, Email = "jondoe@mail.com" },
                new UserViewModel { Name = "Alice", LastName = "Jones", IsHead = false, Email = "alice@mail.com" }
            };
            
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> StepTwo([FromBody]List<UserViewModel> regVm) //The fuckin binding doesn't work well in the mvc 6
        {
            //TODO: Put data into DB

            //GOTO step 3
            return View(regVm);
        } 
    }
}
