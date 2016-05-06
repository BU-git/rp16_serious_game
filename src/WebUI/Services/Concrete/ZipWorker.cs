using System;
using System.Threading.Tasks;
using BLL.Abstract;
using Domain;
using RestSharp;
using RestSharp.Authenticators;
using WebUI.Services.Abstract;

namespace WebUI.Services.Concrete
{
    public class ZipWorker : IZipWorker
    {
        private readonly IRestClient _restClient;

        public ZipWorker(IRestClient restClient, IPropertyConfigurator config)
        {
            var configurator = config;
            _restClient = restClient;
            _restClient.BaseUrl = new Uri(configurator.Get<string>("RestClient", "ZipClient", "Address"));
            _restClient.Authenticator = new HttpBasicAuthenticator(configurator.Get<string>("RestClient", "ZipClient", "Key"),
                configurator.Get<string>("RestClient", "ZipClient", "Secret"));
        }

        public async Task<NlAddress> GetAddressAsync(string zipCode, int houseNumber, string houseNumberAddition = "")
        {
            try
            {
                var request = new RestRequest($"rest/addresses/{zipCode}/{houseNumber}/{houseNumberAddition}",
                    Method.GET);
                var result = await _restClient.ExecuteGetTaskAsync<NlAddress>(request);
                return result.Data;
            }
            catch (Exception)
            {
                throw new Exception("Rest request failed");
            }
        }
    }
}
