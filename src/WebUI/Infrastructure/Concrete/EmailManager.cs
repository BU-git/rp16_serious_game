using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<bool> SendRegistrationMailAsync(Guid id, string address)
        {
            //StringBuilder body = new StringBuilder();

            //body.Append("<!DOCTYPE html>\r\n<html>\r\n<head></head>\r\n<body>Continue registration by clicking <a href=\"");
            //body.Append("localhost:5000/Registration/StepTwo/"); //TODO: Change somehow
            //body.Append(id);
            //body.Append("\">here</a></body>\r\n</html>");

            string subj = "Serious games registration";

            string body = "Continue registration by clicking <a href=\"" +
                "localhost:5000/Registration/StepTwo/" + id + "\">here</a>";


            var result = await _mailSender.SendMailAsync(subj, body, address);

            return result;
        }
    }
}
