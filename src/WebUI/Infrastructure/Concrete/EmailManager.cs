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
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Abstractions;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using WebUI.Infrastructure.Abstract;
using WebUI.ViewModels.Email;
using WebUI.ViewModels.Registration;

namespace WebUI.Infrastructure.Concrete
{
    public class EmailManager : IMailManager
    {
        private readonly IMailSender _mailSender;
        private readonly RazorComposer _razorComposer;
        private readonly IConfigurationRoot _configuration;
        private readonly AbstractEmailBuilder _emailBuilder;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public EmailManager(IMailSender mailSender, RazorComposer razorComposer, IConfigurationRoot configuration, AbstractEmailBuilder emailBuilder, IHttpContextAccessor contextAccessor)
        {
            _mailSender = mailSender;
            _razorComposer = razorComposer;
            _configuration = configuration;
            _emailBuilder = emailBuilder;
            _httpContextAccessor = contextAccessor;
        }

        public async Task<bool> SendRegistrationMailAsync(RegistrationMessage registrationMessage, string addressTo)
        {
            var ipConfigs = GetIpConfiguration();
            registrationMessage.Host = $"{ipConfigs.HostName}:{ipConfigs.Port}"; 

            _emailBuilder.CreateNewMessage();
            _emailBuilder.SetSubject(GetSubjectFromConfig(MailType.Registration));
            _emailBuilder.SetAddressees(addressTo);

            var folder = "..\\" + _configuration.Get<string>("EmailTypes:Folder") + "\\" +
                                      _configuration.Get<string>("EmailTypes:Registration:FileName");
            //var path =
               // Path.GetFullPath(folder);
            //var messageBody = "";//_razorComposer.ComposeStringFromRazor<RegistrationMessage>(path, registrationMessage);

            var path = "~/" + _configuration.Get<string>("EmailTypes:Folder") + "/" + _configuration.Get<string>("EmailTypes:Registration:FileName");
            var viewDataDictionary = new ViewDataDictionary(new Microsoft.AspNet.Mvc.ModelBinding.EmptyModelMetadataProvider(), new Microsoft.AspNet.Mvc.ModelBinding.ModelStateDictionary());
            var actionContext = new ActionContext(_httpContextAccessor.HttpContext, new Microsoft.AspNet.Routing.RouteData(), new ActionDescriptor());
            viewDataDictionary.Model = registrationMessage;
            var text = await _razorComposer.RenderView(path, viewDataDictionary, actionContext, _httpContextAccessor);

            _emailBuilder.SetBody(text);

            bool result = await _mailSender.SendMailAsync(_emailBuilder.GetMailMessage());
            
            return result;
        }

        private HostConfiguration GetIpConfiguration()
        {
            HostConfiguration hostConfig = new HostConfiguration();

            hostConfig.Port = _configuration.Get<int>("ServerConfig:Port");
            hostConfig.HostName = _configuration.Get<string>("ServerConfig:HostName");
            hostConfig.IpAddress = _configuration.Get<string>("ServerConfig:IpAddress");

            return hostConfig;
        }

        private string GetSubjectFromConfig(MailType mailType)
        {
            switch (mailType)
            {
                case MailType.Registration:
                    return _configuration.Get<string>("EmailTypes:Registration:Subject");
                case MailType.Confirmation:
                    return _configuration.Get<string>("EmailTypes:PassConfirm:Subject");
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
