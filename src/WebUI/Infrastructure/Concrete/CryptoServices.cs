using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using NUnit.Framework.Internal;
using WebUI.Infrastructure.Abstract;

namespace WebUI.Infrastructure.Concrete
{
    public class CryptoServices : ICryptoServices
    {
        public string GenerateRandomPassword()
        {
            return System.Web.Security.Membership.GeneratePassword(10, 0);
        }
    }
}
