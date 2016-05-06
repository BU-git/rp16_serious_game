using System;
using System.Threading.Tasks;
using BLL.Abstract;
using Domain;
using WebUI.Infrastructure.Abstract;
using WebUI.Services.Abstract;
using WebUI.ViewModels.Email;

namespace WebUI.Services.Concrete
{
    public class EmailManager : IMailManager
    {
        private readonly IMailSender _mailSender;
        private readonly IViewComposer _viewComposer;
        private readonly IPropertyConfigurator _config;
        private readonly AbstractEmailBuilder _emailBuilder;

        private const string EmailTypes = "EmailTypes";
        private const string Folder = "Folder";
        private const string Registration = "Registration";
        private const string FileName = "FileName";
        private const string ServerConfig = "ServerConfig";
        private const string Port = "Port";
        private const string HostName = "HostName";
        private const string IpAddress = "IpAddress";
        private const string Subject = "Subject";
        private const string PassConfirm = "PassConfirm";

        public EmailManager(IMailSender mailSender, IViewComposer viewComposer, IPropertyConfigurator configurator, AbstractEmailBuilder emailBuilder)
        {
            _mailSender = mailSender;
            _viewComposer = viewComposer;
            _config = configurator;
            _emailBuilder = emailBuilder;
        }

        /// <summary>
        /// Sends email for just registrated person to <see cref="addressTo"/> email with data in registrationMessage
        /// </summary>
        /// <param name="registrationMessage">View model to be rendered at view</param>
        /// <param name="addressTo">Destination email address</param>
        /// <returns>Boolean result of mail message sending</returns>
        public async Task<bool> SendRegistrationMailAsync(RegistrationMessage registrationMessage, string addressTo)
        {
            var ipConfigs = GetIpConfiguration();
            registrationMessage.Host = $"{ipConfigs.HostName}:{ipConfigs.Port}"; 

            _emailBuilder.CreateNewMessage();
            _emailBuilder.SetSubject(GetSubjectFromConfig(MailType.Registration));
            _emailBuilder.SetAddressees(addressTo);

            var path = $"~/{_config.Get<string>(EmailTypes, Folder)}/{_config.Get<string>(EmailTypes, Registration, FileName)}";
            var body = await _viewComposer.RenderView(path, registrationMessage);
            _emailBuilder.SetBody(body);

            var result = await _mailSender.SendMailAsync(_emailBuilder.GetMailMessage());
            
            return result;
        }

        private HostConfiguration GetIpConfiguration()
        {
            var hostConfig = new HostConfiguration
            {
                Port = _config.Get<int>(ServerConfig, Port),
                HostName = _config.Get<string>(ServerConfig, HostName),
                IpAddress = _config.Get<string>(ServerConfig, IpAddress)
            };


            return hostConfig;
        }

        private string GetSubjectFromConfig(MailType mailType)
        {
            switch (mailType)
            {
                case MailType.Registration:
                    return _config.Get<string>(EmailTypes, Registration, Subject);
                case MailType.Confirmation:
                    return _config.Get<string>(EmailTypes, PassConfirm, Subject);
                default:
                    return String.Empty;
            }
        }

        public enum MailType
        {
            Registration,
            Confirmation
        };
    }
}
