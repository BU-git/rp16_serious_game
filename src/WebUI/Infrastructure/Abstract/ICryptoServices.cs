using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Infrastructure.Abstract
{
    public interface ICryptoServices
    {
        string GenerateRandomPassword();
        string GenerateRandomAlphanumericString(int length);
    }
}
