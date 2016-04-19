using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUI.Services
{
    class JsonTranslationProvider : ITranslationProvider
    {
        public IEnumerable<string> Languages { get; private set; }

        private readonly IDictionary<Tuple<string, string>,string>  _translations;
        public string Translate(string key)
        {
            return _translations[Tuple.Create(key, TranslationManager.Instance.CurrentLanguage)] ?? _translations[Tuple.Create(key, "en")];
        }

        public JsonTranslationProvider(string fileName)
        {

            using (StreamReader strm = new StreamReader(File.OpenRead(fileName), Encoding.UTF8))
            {
                JObject obj = JObject.Parse(strm.ReadToEnd());
                
                if (obj.Count > 0)
                {                   
                    _translations = new Dictionary<Tuple<string, string>, string>();
                    foreach (KeyValuePair<string, JToken> item in obj)
                    {
                        
                        Dictionary<string, string> dictionary = obj[item.Key].ToObject<Dictionary<string, string>>();
                        
                        
                        foreach (KeyValuePair<string, string> lang in dictionary)
                        {
                            KeyValuePair<Tuple<string, string>, string> rec = new KeyValuePair<Tuple<string, string>, string>(Tuple.Create(item.Key, lang.Key), lang.Value);
                            _translations.Add(rec);
                        }


                    }
                }

                Languages = _translations.Keys.Select(t => t.Item2).Distinct();
            }          
        }
    }
}
