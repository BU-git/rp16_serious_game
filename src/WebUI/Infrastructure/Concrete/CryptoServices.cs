using System;
using System.Linq;
using WebUI.Infrastructure.Abstract;

namespace WebUI.Infrastructure.Concrete
{
    public class CryptoServices : ICryptoServices
    {
        public string GenerateRandomPassword()
        {
            return System.Web.Security.Membership.GeneratePassword(10, 0) + new Random().Next(100);
        }

        public string GenerateRandomAlphanumericString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
