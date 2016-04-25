using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using WebUI.Infrastructure.Abstract;
using WebUI.ViewModels.Registration;
using Interfaces;
using Microsoft.AspNet.Authorization;
using WebUI.ViewModels.Email;
using Gender = Domain.Entities.Gender;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Controllers
{
    [Authorize]
    public class RegistrationController : Controller
    {
        private readonly IMailManager _mailManager;
        private readonly ICryptoServices _cryptoServices;
        private readonly IDAL _dal;
        private readonly SignInManager<ApplicationUser> _signInManager;
        
        public RegistrationController(IMailManager mailManager, ICryptoServices crypto, IDAL dal, SignInManager<ApplicationUser> signInManager)
        {
            _mailManager = mailManager;
            _cryptoServices = crypto;
            _dal = dal;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult StepOne()
        {
            return View(new MainFamilyData());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StepOne([FromForm]MainFamilyData regVm)
        {
            if (!ModelState.IsValid)
            {
                return View(regVm);
            }

            string randomPass = _cryptoServices.GenerateRandomPassword();
            string randomUserName = _cryptoServices.GenerateRandomAlphanumericString(6);
            UserGroup group = new UserGroup
            {
                GroupName = regVm.FamilyName,
                Type = regVm.FamilyType
            };
            await _dal.CreateUserGroup(group);

            ApplicationUser user = new ApplicationUser
            {
                LastName = regVm.FamilyName,
                Email = regVm.HeadEmail,
                UserName = randomUserName
            };
            await _dal.CreateParticipant(user, randomPass); //TODO: show message

            var registrationMessage = new RegistrationMessage
            {
                Password = randomPass,
                Name = regVm.FamilyName,
                Login = regVm.HeadEmail,
                LinkUrl = Url.Action("StepTwo")
            };

            await _mailManager.SendRegistrationMailAsync(registrationMessage, regVm.HeadEmail);

            return View(new MainFamilyData());
        }

        [HttpGet]
        public async Task<IActionResult> StepTwo(string familyName)
        {
            //TODO: get main data 'bout family 
            //Can't get information about family because dal doesn't contain methods like GetFamilyByName(string familyName)

            FamilyViewModel familyInfo = new FamilyViewModel
            {
                Users = new List<UserViewModel>
                {
                    new UserViewModel()
                },
                FamilyName = "Doe"
                //Here must be users
            };

            return View(familyInfo);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> StepTwo(FamilyViewModel regVm)
        {
            foreach (UserViewModel u in regVm.Users)
            {
                string randomPass = _cryptoServices.GenerateRandomPassword();
                Gender gender;
                Enum.TryParse(u.Gender.ToString(), out gender);
                DateTime dateTime;
                DateTime.TryParse($"{u.Day}/{u.Month}/{u.Year}", out dateTime);

                ApplicationUser user = new ApplicationUser();
                user.Name = u.Name;
                user.MiddleName = u.MiddleName;
                user.LastName = regVm.FamilyName;
                user.BirthDate = dateTime;
                user.Email = u.Email;
                user.Phone = u.Phone;
                user.Gender = gender;
                user.ZipCode = regVm.ZipCode;
                user.Street = regVm.Street;
                user.Country = regVm.Country;
                user.Region = regVm.Region;
                user.City = regVm.City;
                user.BuildingNumber = regVm.BuildingNumber;

                try
                {
                    await _dal.CreateParticipant(user, randomPass); //TODO: The try/catch block shouldn't be here!
                }
                catch (Exception)
                {
                    return View(regVm);
                }

                var registrationMessage = new RegistrationMessage
                {
                    Login = u.Email,
                    Name = u.Name,
                    Password = randomPass
                };
                await _mailManager.SendRegistrationMailAsync(registrationMessage, u.Email);
            }
            
            //TODO: assign members to concrete family considering previous comment about DAL
            //TODO: add success message

            return await StepTwo("");
        }

        [HttpPost]
        public PartialViewResult RegistrationForm()
        {
            return PartialView("_UserDetails", new UserViewModel());
        }
    }
}
