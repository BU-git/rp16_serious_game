using System.Net.Mail;
using System.Threading.Tasks;

namespace BLL.Abstract
{
    public interface IMailSender
    {
        Task<bool> SendMailAsync(MailMessage message);
    }
}
