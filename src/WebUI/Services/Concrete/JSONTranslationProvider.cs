using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using WebUI.Services.Abstract;

namespace WebUI.Services.Concrete
{
    class JsonTranslationProvider : ITranslationProvider
    {
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        public IEnumerable<string> Languages { get; private set; }

        private readonly IDictionary<Tuple<string, string>,string>  _translations;
        public string Translate(string key)
        {
            return _translations[Tuple.Create(key, TranslationManager.Instance.CurrentLanguage)] ?? _translations[Tuple.Create(key, "en")];
        }

        public JsonTranslationProvider(string fileName)
        {

            using (var strm = new StreamReader(File.OpenRead(fileName), Encoding.UTF8))
            {
                var obj = JObject.Parse(strm.ReadToEnd());
                
                if (obj.Count > 0)
                {                   
                    _translations = new Dictionary<Tuple<string, string>, string>();
                    foreach (var item in obj)
                    {
                        
                        var dictionary = obj[item.Key].ToObject<Dictionary<string, string>>();
                        
                        
                        foreach (var lang in dictionary)
                        {
                            var rec = new KeyValuePair<Tuple<string, string>, string>(Tuple.Create(item.Key, lang.Key), lang.Value);
                            _translations.Add(rec);
                        }


                    }
                }

                Languages = _translations.Keys.Select(t => t.Item2).Distinct();
            }          
        }
    }
}
