using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Infrastructure.Abstract
{
    public interface IMailManager
    {
        Task<bool> SendRegistrationMailAsync(Guid id, string address);
    }
}
