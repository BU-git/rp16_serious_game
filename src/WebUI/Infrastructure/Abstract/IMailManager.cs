using System.Threading.Tasks;

namespace WebUI.Infrastructure.Abstract
{
    public interface IMailManager
    {
        Task<bool> SendRegistrationMailAsync(string password, string address);
    }
}
