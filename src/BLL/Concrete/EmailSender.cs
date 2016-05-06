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
        private readonly IPropertyConfigurator _config;

        private const string Email = "Email";
        private const string Address = "Address";
        private const string Password = "Password";
        private const string Username = "Username";
        private const string UseSsl = "UseSsl";
        private const string Port = "Port";
        private const string Host = "Host";

        public EmailSender(IPropertyConfigurator configurator)
        {
            _config = configurator;
        }

        public async Task<bool> SendMailAsync(MailMessage message)
        {
            var credential = GetCredentialsFromConfig();

            message.IsBodyHtml = true;
            message.From = new MailAddress(credential.Email);
            
            try
            {
                using (var client = new SmtpClient(credential.Host, credential.Port)
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
            var credentials = new EmailCredential
            {
                Email = _config.Get<string>(Email, Address),
                Password = _config.Get<string>(Email, Password),
                Username = _config.Get<string>(Email, Username),
                UseSsl = _config.Get<bool>(Email, UseSsl),
                Port = _config.Get<int>(Email, Port),
                Host = _config.Get<string>(Email, Host)
            };


            return credentials;
        }
    }
}