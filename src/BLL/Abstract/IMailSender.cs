using System.Threading.Tasks;

namespace BLL.Abstract
{
    public interface IMailSender
    {
        Task<bool> SendMail(string subject, string body, string emailTo);
    }
}
