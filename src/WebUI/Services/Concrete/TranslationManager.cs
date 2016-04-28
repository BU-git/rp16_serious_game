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

        public static TranslationManager Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly TranslationManager instance = new TranslationManager();
        }


        public ITranslationProvider TranslationProvider { get; set; }
       

        public IEnumerable<string> Languages
        {
            get
            {
                return TranslationProvider?.Languages ?? Enumerable.Empty<string>();
            }
        }

        private string _currentLanguage;
        public string CurrentLanguage
        {
            get
            {
                return _currentLanguage ?? (_currentLanguage = "en");
            }
            set
            {
                if (value != _currentLanguage)
                {
                    _currentLanguage = value;

                }
            }
        }

        public string Translate(string key)
        {
            return TranslationProvider?.Translate(key) ?? string.Format("!{0}!", key);
        }

    }
}
