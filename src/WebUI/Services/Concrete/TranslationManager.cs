using System.Collections.Generic;
using System.Linq;
using WebUI.Services.Abstract;

namespace WebUI.Services.Concrete
{
    class TranslationManager
    {
        
        private TranslationManager()
        {
        }

        public static TranslationManager Instance => Nested.Instance;

        private class Nested
        {
            static Nested()
            {
            }

             internal static readonly TranslationManager Instance = new TranslationManager();
        }


        public ITranslationProvider TranslationProvider { get; set; }
       

        public IEnumerable<string> Languages => TranslationProvider?.Languages ?? Enumerable.Empty<string>();

        private string _currentLanguage;
        public string CurrentLanguage
        {
            get
            {
                return _currentLanguage ?? (_currentLanguage = "en");
            }
            set
            {
                if (_currentLanguage != null && value != _currentLanguage)
                {
                    _currentLanguage = value;

                }
            }
        }

        public string Translate(string key)
        {
            return TranslationProvider?.Translate(key) ?? $"!{key}!";
        }

    }
}
