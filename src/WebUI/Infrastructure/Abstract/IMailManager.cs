using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.ViewModels.Email;

namespace WebUI.Infrastructure.Abstract
{
    public interface IMailManager
    {
        Task<bool> SendRegistrationMailAsync(RegistrationMessage registrationMessage, string addressTo);
    }
}
