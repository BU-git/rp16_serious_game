#region
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using BLL.Abstract;
using Domain;
using Microsoft.Extensions.Configuration;

#endregion

namespace BLL.Concrete
{
    public class EmailSender : IMailSender
    {
        private readonly IConfigurationRoot _configuration;

        public EmailSender(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendMailAsync(MailMessage message)
        {
            var credential = GetCredentialsFromConfig();

            message.IsBodyHtml = true;
            message.From = new MailAddress(credential.Email);
            
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
                {
                    await client.SendMailAsync(message);
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
            EmailCredential credentials = new EmailCredential();

            credentials.Email = _configuration.Get("Email:Address");
            credentials.Password = _configuration.Get("Email:Password");
            credentials.Username = _configuration.Get("Email:Username");
            credentials.UseSsl = bool.Parse(_configuration.Get("Email:UseSsl"));
            credentials.Port = int.Parse(_configuration.Get("Email:Port"));
            credentials.Host = _configuration.Get("Email:Host");

            return credentials;
        }
    }
}