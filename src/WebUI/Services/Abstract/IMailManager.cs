using System.Threading.Tasks;
using WebUI.ViewModels.Email;

namespace WebUI.Services.Abstract
{
    public interface IMailManager
    {
        Task<bool> SendRegistrationMailAsync(RegistrationMessage registrationMessage, string addressTo);
    }
}
