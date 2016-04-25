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

            credentials.Email = _configuration.Get<string>("Email:Address");
            credentials.Password = _configuration.Get<string>("Email:Password");
            credentials.Username = _configuration.Get<string>("Email:Username");
            credentials.UseSsl = _configuration.Get<bool>("Email:UseSsl");
            credentials.Port = _configuration.Get<int>("Email:Port");
            credentials.Host = _configuration.Get<string>("Email:Host");

            return credentials;
        }
    }
}