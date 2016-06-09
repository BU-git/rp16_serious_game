using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNet.Mvc;
using WebUI.Infrastructure.Abstract;
using WebUI.ViewModels.Registration;
using Interfaces;
using Microsoft.AspNet.Authorization;
using WebUI.Services.Abstract;
using WebUI.ViewModels.Email;
using Gender = Domain.Entities.Gender;

namespace WebUI.Controllers
{
    [Authorize]
    public class RegistrationController : Controller
    {
        private readonly IMailManager _mailManager;
        private readonly ICryptoServices _cryptoServices;
        private readonly IDAL _dal;

        public RegistrationController(IMailManager mailManager, ICryptoServices crypto, IDAL dal)
        {
            _mailManager = mailManager;
            _cryptoServices = crypto;
            _dal = dal;
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

            var randomPass = _cryptoServices.GenerateRandomPassword();

            var group = new UserGroup
            {
                GroupName = regVm.FamilyName,
                Type = regVm.FamilyType
            };
            await _dal.CreateUserGroup(group);

            var user = new ApplicationUser
            {
                LastName = regVm.FamilyName,
                Email = regVm.HeadEmail
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
#pragma warning disable 1998
        public async Task<IActionResult> StepTwo(string familyName)
#pragma warning restore 1998
        {
            //TODO: get main data 'bout family 
            //Can't get information about family because dal doesn't contain methods like GetFamilyByName(string familyName)

            var familyInfo = new FamilyViewModel
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
            foreach (var u in regVm.Users)
            {
                var randomPass = _cryptoServices.GenerateRandomPassword();
                Gender gender;
                Enum.TryParse(u.Gender.ToString(), out gender);
                DateTime dateTime;
                DateTime.TryParse($"{u.Day}/{u.Month}/{u.Year}", out dateTime);

                var user = new ApplicationUser
                {
                    Name = u.Name,
                    MiddleName = u.MiddleName,
                    LastName = regVm.FamilyName,
                    BirthDate = dateTime,
                    Email = u.Email,
                    Phone = u.Phone,
                    Gender = gender,
                    ZipCode = regVm.ZipCode,
                    Street = regVm.Street,
                    Country = regVm.Country,
                    Region = regVm.Region,
                    City = regVm.City,
                    BuildingNumber = regVm.BuildingNumber
                };

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
            return await StepThree();
        }

        [HttpPost]
        public PartialViewResult RegistrationForm()
        {
            return PartialView("_UserDetails", new UserViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> StepThree()
        {
            var avatars = await _dal.GetAllAvatarsWithPrice(0);
            var model = new AvatarsViewModel { Avatars = avatars };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> StepThree(int avatarId)
        {
            var user = await GetCurrentUserAsync();
            var avatar = await _dal.GetAvatarById(avatarId);
            await _dal.UpdateUserAvatar(avatar, user);
            return RedirectToAction("Index", "Home");
        }


        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _dal.GetUserByEmail(HttpContext.User.Identity.Name);
        }
    }
}
