#region

using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

#endregion

namespace WebUI.Infrastructure.Concrete
{
    public class EmailSender
    {
        public async Task<bool> SendMail(string subject, string body, string emailTo)
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

        private EmailCredential GetCredentialsHardcoded()
        {
            EmailCredential credential = new EmailCredential
            {
                //TODO: set credential properties
            };

            return credential;
        }

        private EmailCredential GetCredentialsFromConfig()
        {
            EmailCredential credential = new EmailCredential
            {
                Host = ConfigurationManager.AppSettings["email.credentials.host"],
                Password = ConfigurationManager.AppSettings["email.credentials.pass"],
                Port = int.Parse(ConfigurationManager.AppSettings["email.credentials.port"]),
                UseSsl = bool.Parse(ConfigurationManager.AppSettings["email.credentials.ssl"]),
                Username = ConfigurationManager.AppSettings["email.credentials.user"],
                Email = ConfigurationManager.AppSettings["email.credentials.email"]
            };

            return credential;
        }
    }
}