using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Entities;
using Interfaces;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using WebUI.ViewModels.AvatarMarket;

namespace WebUI.Controllers
{
    [Authorize]
    public class MarketplaceController : Controller
    {
        private readonly IDAL _dal;
        private readonly UserManager<ApplicationUser> _userManager;

        public MarketplaceController(IDAL dal, UserManager<ApplicationUser> userManager)
        {
            _dal = dal;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> AvatarList()
        {
            var user = await GetCurrentUserAsync();
            var avatars = await _dal.FindNotAvailableAvatars(user);
            var avatarsViewModel = new AvatarsManyViewModel();
            foreach (var avatar in avatars)
            {
                avatarsViewModel.AvatarViewModels.Add(new AvatarViewModel { Avatar = avatar });
            }
            return View(avatarsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AvatarList(int avatarId)
        {
            var avatar = await _dal.GetAvatarById(avatarId);
            var user = await GetCurrentUserAsync();
            if (avatar.Price <= user.Coins)
            {
                user.Coins -= avatar.Price;
                await _dal.UpdateUserAvailableAvatars(avatar, user);
                await _userManager.UpdateAsync(user);
                return await AvatarList();
            }
            return await AvatarList();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            AddLoginSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            UpdateUserSuccess,
            Error
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.FindByIdAsync(HttpContext.User.GetUserId());
        }

        #endregion
    }
}
