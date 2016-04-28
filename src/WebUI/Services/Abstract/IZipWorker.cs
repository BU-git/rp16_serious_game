using System.Threading.Tasks;
using Domain;

namespace WebUI.Services.Abstract
{
    public interface IZipWorker
    {
        Task<NlAddress> GetAddressAsync(string zipCode, int houseNumber, string houseNumberAddition = "");
    }
}
