#region
using System.Configuration;
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

            MailMessage mailToClient = new MailMessage(credential.Email, emailTo)
            {
                Body = body,
                Subject = subject,
                IsBodyHtml = true
            };

            try
            {
                using (SmtpClient client = new SmtpClient(credential.Host, credential.Port)
                {
                    EnableSsl = credential.UseSsl,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                })
                {
                    client.Credentials = new NetworkCredential(credential.Username, credential.Password);

                    await client.SendMailAsync(mailToClient);
                }
            }
            catch (SmtpException exc)
            {
                return false;
            }
            return true;
        }

        private EmailCredential GetCredentialsFromJson()
        {
            EmailCredential credential = new EmailCredential();

            return credential;
        }

        private EmailCredential GetCredentialsFromConfig()
        {
            EmailCredential credential = new EmailCredential
            {
                
            };

            return credential;
        }
    }
}