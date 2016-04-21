using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BLL.Abstract;
using Domain;
using Microsoft.Extensions.Configuration;
using WebUI.Infrastructure.Abstract;
using WebUI.ViewModels.Email;

namespace WebUI.Infrastructure.Concrete
{
    public class EmailManager : IMailManager
    {
        private readonly IMailSender _mailSender;
        private readonly RazorComposer _razorComposer;
        private readonly IConfigurationRoot _configuration;
        private readonly AbstractEmailBuilder _emailBuilder;

        
        public EmailManager(IMailSender mailSender, RazorComposer razorComposer, IConfigurationRoot configuration, AbstractEmailBuilder emailBuilder)
        {
            _mailSender = mailSender;
            _razorComposer = razorComposer;
            _configuration = configuration;
            _emailBuilder = emailBuilder;
        }

        public async Task<bool> SendRegistrationMailAsync(RegistrationMessage registrationMessage, string addressTo)
        {
            var ipConfigs = GetIpConfiguration();
            registrationMessage.Host = $"{ipConfigs.HostName}:{ipConfigs.Port}"; 

            _emailBuilder.CreateNewMessage();
            _emailBuilder.SetSubject(GetSubjectFromConfig(MailType.Confirmation));
            _emailBuilder.SetAddressees(null, addressTo);

            var path =
                Path.GetDirectoryName(_configuration.Get("EmailTypes:Folder") +
                                      _configuration.Get("EmailTypes:Registration:FileName"));
            var messageBody = _razorComposer.ComposeStringFromRazor(path, registrationMessage);

            _emailBuilder.SetBody(messageBody);

            bool result = await _mailSender.SendMailAsync(_emailBuilder.GetMailMessage());

            return result;
        }

        private HostConfiguration GetIpConfiguration()
        {
            HostConfiguration hostConfig = new HostConfiguration();

            hostConfig.Port = int.Parse(_configuration.Get("ServerConfig:Port"));
            hostConfig.HostName = _configuration.Get("ServerConfig:HostName");
            hostConfig.IpAddress = _configuration.Get("ServerConfig:IpAddress");

            return hostConfig;
        }

        private string GetSubjectFromConfig(MailType mailType)
        {
            switch (mailType)
            {
                case MailType.Registration:
                    return _configuration.Get("EmailTypes:Registration:Subject");
                case MailType.Confirmation:
                    return _configuration.Get("EmailTypes:PassConfirm:Subject");
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
