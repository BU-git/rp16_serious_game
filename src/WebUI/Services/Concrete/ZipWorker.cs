using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Abstract;
using Domain;
using Microsoft.Framework.ConfigurationModel;
using RestSharp;
using RestSharp.Authenticators;
using WebUI.Services.Abstract;

namespace WebUI.Services.Concrete
{
    public class ZipWorker : IZipWorker
    {
        private readonly IRestClient _restClient;
        private readonly IPropertyConfigurator _config;

        public ZipWorker(IRestClient restClient, IPropertyConfigurator config)
        {
            this._config = config;
            this._restClient = restClient;
            _restClient.BaseUrl = new Uri(_config.Get<string>("RestClient", "ZipClient", "Address"));
            _restClient.Authenticator = new HttpBasicAuthenticator(_config.Get<string>("RestClient", "ZipClient", "Key"),
                _config.Get<string>("RestClient", "ZipClient", "Secret"));
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
