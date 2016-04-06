using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Services
{
    class JSONTranslationProvider : ITranslationProvider
    {
        private IEnumerable<CultureInfo> languages;
        public IEnumerable<CultureInfo> Languages
        {
            get { return languages; }
        }

        private IDictionary<Tuple<string, string>,string>  translations;
        public object Translate(string key)
        {
            return translations[Tuple.Create(key, TranslationManager.Instance.CurrentLanguage.Name)] ?? translations[Tuple.Create(key, "en")];
        }

        public JSONTranslationProvider(string fileName)
        {
            
            using (StreamReader strm = new StreamReader(fileName))
            {
                JObject obj = JObject.Parse(strm.ReadToEnd());
                
                if (obj.Count > 0)
                {                   
                    translations = new Dictionary<Tuple<string, string>, string>();
                    foreach (KeyValuePair<string, JToken> item in obj)
                    {
                        
                        var dictionary = obj[item.Key].ToObject<Dictionary<string, string>>();
                        
                        
                        foreach (var lang in dictionary)
                        {
                            var rec = new KeyValuePair<Tuple<string, string>, string>(Tuple.Create(item.Key, lang.Key), lang.Value);
                            translations.Add(rec);
                        }


                    }
                }


                languages = translations.Keys.Select(t => new CultureInfo(t.Item2)).Distinct();

            }

           
        }
    }
}
