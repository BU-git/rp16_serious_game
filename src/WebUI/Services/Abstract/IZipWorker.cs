using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace WebUI.Services.Abstract
{
    public interface IZipWorker
    {
        Task<NlAddress> GetAddressAsync(string zipCode, int houseNumber, string houseNumberAddition = "");
    }
}
