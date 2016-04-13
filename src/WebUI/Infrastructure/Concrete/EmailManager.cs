using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BLL.Abstract;
using WebUI.Infrastructure.Abstract;

namespace WebUI.Infrastructure.Concrete
{
    public class EmailManager : IMailManager
    {
        private readonly IMailSender _mailSender;
        
        public EmailManager(IMailSender mailSender)
        {
            _mailSender = mailSender;
        }

        public async Task<bool> SendRegistrationMailAsync(string password, string address)
        {
            StringBuilder body = new StringBuilder();
            string host = Dns.GetHostName();
            string subj = "You were successfully registered to Serious Games";
            
            body.Append("<!DOCTYPE html>\r\n<html>\r\n<head></head>\r\n<body><p>Your login: ");
            body.Append(address);
            body.Append("</p><p>Your password: ");
            body.Append(password);
            body.Append("</p><p>Continue registration by clicking <a href=\"");
            body.Append("http://localhost:51842");
            body.Append("/Registration/StepTwo/\">here</a></p></body>\r\n</html>");
            
            var result = await _mailSender.SendMailAsync(subj, body.ToString(), address);

            return result;
        }
    }
}
