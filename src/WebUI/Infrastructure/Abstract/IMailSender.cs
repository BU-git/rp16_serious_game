using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Infrastructure.Abstract
{
    public interface IMailSender
    {
        Task<bool> SendMail(string subject, string body, string emailTo);
    }
}
