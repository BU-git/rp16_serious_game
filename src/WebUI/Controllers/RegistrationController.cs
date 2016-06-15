using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNet.Mvc;
using WebUI.Infrastructure.Abstract;
using WebUI.ViewModels.Registration;
using Interfaces;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http.Extensions;
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
        [Authorize(Roles = "Coach")]
        public IActionResult StepOne()
        {
            return View(new MainFamilyData());
        }

        [HttpPost]
        [Authorize(Roles = "Coach")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StepOne([FromForm]MainFamilyData regVm)
        {
            if (!ModelState.IsValid)
            {
                return View(regVm);
            }
            if (!User.IsInRole("Coach"))
            {
                TempData["warn"] = "Access denied.";
                return RedirectToAction("TaskList", "Task");
            }

            var randomPass = _cryptoServices.GenerateRandomPassword();

            var user = new ApplicationUser
            {
                LastName = regVm.FamilyName,
                Email = regVm.HeadEmail,
                UserName = regVm.HeadEmail
            };
            await _dal.CreateParticipant(user, randomPass);

            var group = new UserGroup
            {
                GroupName = regVm.FamilyName,
                Type = regVm.FamilyType
            };
            await _dal.CreateUserGroup(group);
            await _dal.AddUserToGroup(user, group);

            var userGroup = await _dal.GetUserGroupByName(regVm.FamilyName);
            var registrationMessage = new RegistrationMessage
            {
                Password = randomPass,
                Name = regVm.FamilyName,
                Login = regVm.HeadEmail,
                LinkUrl = $"{Url.Action("StepTwo")}/{userGroup.UserGroupId}"
            };

            await _mailManager.SendRegistrationMailAsync(registrationMessage, regVm.HeadEmail);

            return View(new MainFamilyData());
        }

        [HttpGet]
        public async Task<IActionResult> StepTwo(int familyId)
        {
            var userGroup = await _dal.GetUserGroupById(familyId);
            var currentUser = await GetCurrentUserAsync();

            if (User.IsInRole("Coach") || userGroup == null)
            {
                TempData["warn"] = "Access denied.";
                return RedirectToAction("TaskList", "Task");
            }

            if (!_dal.GetUserGroupUsers(userGroup).Contains(currentUser))
            {
                TempData["warn"] = "Access denied.";
                return RedirectToAction("TaskList", "Task");
            }

            return View(new FamilyViewModel(userGroup));
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> StepTwo(FamilyViewModel regVm)
        {
            if (User.IsInRole("Coach"))
            {
                TempData["warn"] = "Access denied.";
                return RedirectToAction("TaskList", "Task");
            }

            var userGroup = await _dal.GetUserGroupById(regVm.FamilyId);

            foreach (var u in regVm.Users)
            {
                var randomPass = _cryptoServices.GenerateRandomPassword();
                Gender gender;
                Enum.TryParse(u.Gender.ToString(), out gender);
                DateTime dateTime;
                DateTime.TryParse($"{u.Day}/{u.Month}/{u.Year}", out dateTime);

                var userInDb = await _dal.GetUserByEmail(u.Email);
                if (userInDb != null)
                {
                    userInDb.Name = u.Name;
                    userInDb.MiddleName = u.MiddleName;
                    userInDb.LastName = regVm.FamilyName;
                    userInDb.BirthDate = dateTime;
                    userInDb.Email = u.Email;
                    userInDb.Phone = u.Phone;
                    userInDb.Gender = gender;
                    userInDb.ZipCode = regVm.ZipCode;
                    userInDb.Street = regVm.Street;
                    userInDb.Country = regVm.Country;
                    userInDb.Region = regVm.Region;
                    userInDb.City = regVm.City;
                    userInDb.BuildingNumber = regVm.BuildingNumber;

                    try
                    {
                        await _dal.EditUser(userInDb);
                    }
                    catch (Exception exc)
                    {
                        TempData["error"] = exc.Message;
                        return View(regVm);
                    }
                }
                else
                {
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
                        BuildingNumber = regVm.BuildingNumber,
                        UserName = u.Email
                    };

                    await _dal.CreateParticipant(user, randomPass);
                    await _dal.AddUserToGroup(user, userGroup);

                    var registrationMessage = new RegistrationMessage
                    {
                        Login = u.Email,
                        Name = u.Name,
                        Password = randomPass,
                        LinkUrl = $"{Url.Action("StepThree")}"
                    };
                    await _mailManager.SendRegistrationMailAsync(registrationMessage, u.Email);
                }
            }

           return RedirectToAction("StepTwo");
        }

        [HttpPost]
        public PartialViewResult RegistrationForm()
        {
            return PartialView("_UserDetails", new UserViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> StepThree()
        {
            if (User.IsInRole("Coach"))
            {
                TempData["warn"] = "Access denied.";
                return RedirectToAction("TaskList", "Task");
            }

            var avatars = await _dal.GetAllAvatarsWithPrice(0);
            return View(avatars);
        }

        [HttpPost]
        public async Task<IActionResult> StepThree(int avatarId)
        {
            if (User.IsInRole("Coach"))
            {
                TempData["warn"] = "Access denied.";
                return RedirectToAction("TaskList", "Task");
            }

            try
            {
                var user = await GetCurrentUserAsync();
                var avatar = await _dal.GetAvatarById(avatarId);
                await _dal.UpdateUserAvailableAvatars(avatar, user);
                await _dal.UpdateUserAvatar(avatar, user);
                return RedirectToAction("TaskList", "Task");
            }
            catch (Exception exc)
            {
                TempData["error"] = exc.Message;
                return await StepThree();
            }
        }


        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _dal.GetUserByEmail(HttpContext.User.Identity.Name);
        }
    }
}
