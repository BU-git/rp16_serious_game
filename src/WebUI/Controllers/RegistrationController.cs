using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BLL.Abstract;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using NUnit.Framework.Constraints;
using WebUI.Infrastructure.Abstract;
using WebUI.ViewModels.Registration;
using Interfaces;
using Microsoft.AspNet.Authorization;
using Gender = Domain.Entities.Gender;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IMailManager _mailManager;
        private readonly ICryptoServices _cryptoServices;
        private readonly IDal _dal;
        private readonly SignInManager<ApplicationUser> _signInManager; 

        public RegistrationController(IMailManager mailManager, ICryptoServices crypto, IDal dal, SignInManager<ApplicationUser> signInManager)
        {
            _mailManager = mailManager;
            _cryptoServices = crypto;
            _dal = dal;
            _signInManager = signInManager;
        }

        public IActionResult StepOne()
        {
            return View(new MainFamilyData());
        }

        [HttpPost]
        [AllowAnonymous]
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
            await _dal.CreateParticipant(user, randomPass);
            await _mailManager.SendRegistrationMailAsync(randomPass, regVm.HeadEmail);

            return View(new MainFamilyData());
        }

        public async Task<IActionResult> StepTwo(string familyName)
        {
            //TODO: get main data 'bout family 
            //Can't get information about family because dal doesn't contain methods like GetFamilyByName(string familyName)

            var familyInfo = new FamilyViewModel
            {
                Users = new List<UserViewModel>
                {
                    new UserViewModel()
                }
                //Here must be users
            };

            return View(familyInfo);
        }

        [HttpPost]
        public async Task<IActionResult> StepTwo([FromForm]FamilyViewModel regVm)
        {
            foreach (var u in regVm.Users)
            {
                var randomPass = _cryptoServices.GenerateRandomPassword();
                Gender gender;
                Enum.TryParse(u.Gender.ToString(), out gender);
                DateTime dateTime;
                DateTime.TryParse($"{u.Day}/{u.Month}/{u.Year}", out dateTime);

                var user = new ApplicationUser();
                user.Name = u.Name;
                user.MidleName = u.MidleName;
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

                await _mailManager.SendRegistrationMailAsync(randomPass, u.Email);
            }
            
            //TODO: assign members to concrete family considering previous comment about DAL
            //TODO: add success message

            return await StepTwo("");
        }

        [HttpPost]
        public PartialViewResult RegistrationForm(int index)
        {
            return PartialView("_StepTwoForm", new UserViewModel { Index = index });
        }
    }
}
