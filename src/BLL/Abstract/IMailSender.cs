using System.Threading.Tasks;

namespace BLL.Abstract
{
    public interface IMailSender
    {
        Task<bool> SendMailAsync(string subject, string body, string emailTo);
    }
}
