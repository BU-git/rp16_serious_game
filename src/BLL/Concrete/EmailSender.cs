#region

using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using BLL.Abstract;
using Domain;

#endregion

namespace BLL.Concrete
{
    public class EmailSender : IMailSender
    {
        public async Task<bool> SendMailAsync(string subject, string body, string emailTo)
        {
            EmailCredential credential = GetCredentialsFromConfig();
            
            try
            {
                using (SmtpClient client = new SmtpClient(credential.Host, credential.Port)
                {
                    EnableSsl = credential.UseSsl,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(credential.Email, credential.Password),
                    Timeout = 2000
                })
                using (MailMessage mailToClient = new MailMessage(credential.Email, emailTo)
                {
                    IsBodyHtml = true,
                    Body = body,
                    Subject = subject
                })
                {
                    await client.SendMailAsync(mailToClient);
                }
            }
            catch (SmtpException)
            {
                return false;
            }
            return true;
        }

        private EmailCredential GetCredentialsFromConfig()
        {
            EmailCredential credential = new EmailCredential
            {
                Email = "rp16.serious.games@gmail.com",
                Password = "1qaz_@WSX_3edc",
                Host = "smtp.gmail.com",
                Port = 587,
                UseSsl = true,
                Username = "rp16.serious.games"
            };

            return credential;
        }
    }
}