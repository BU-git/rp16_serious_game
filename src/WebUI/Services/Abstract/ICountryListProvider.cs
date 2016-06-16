using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Services.Abstract
{
    public interface ICountryListProvider
    {
        Task<List<string>> GetCountries(string region);

    }
}
