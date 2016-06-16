using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Net.Http.Server;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography.Xml;
using Newtonsoft.Json;
using WebUI.Services.Abstract;

namespace WebUI.Services.Concrete
{
    public class RESTCountryListProvider : ICountryListProvider
    {
        private string url = "https://restcountries.eu";



        public async Task<List<string>> GetCountries(string region)
        {
            try
            {


                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync("/rest/v1/subregion/" + region);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        List< Country> _countries = JsonConvert.DeserializeObject<List<Country>> (jsonString);
        
                       List<string> countries = _countries.Select(x => x.name).ToList();
                        return countries;
                    }
                    else
                    {
                        throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).",
                            response.StatusCode));
                    }


                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

    }


    public class Country
    {
        [JsonProperty("name")]
        public string name { get; set; }
    }
}
