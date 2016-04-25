using System.Linq;
using BLL.Abstract;
using Microsoft.Extensions.Configuration;

namespace BLL.Concrete
{
    public class JsonPropertyConfigurator : IPropertyConfigurator
    {
        private readonly IConfigurationRoot _configuration;

        public JsonPropertyConfigurator(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Searches for the value in config file by key that consists of <see cref="keys"/> divided with ':'
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys">Keys to value in json config file</param>
        /// <returns>The value by key if found</returns>
        public T Get<T>(params string[] keys)
        {
            var configKey = keys.Aggregate(string.Empty, (current, key) => current + key + ":").Trim(':');
            return _configuration.Get<T>(configKey);
        }
    }
}
