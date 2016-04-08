using Ninject;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Services
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

        private string currentLanguage;
        public string CurrentLanguage
        {
            get
            {
                return currentLanguage ?? (currentLanguage = "en");
            }
            set
            {
                if (value != currentLanguage)
                {
                    currentLanguage = value;

                }
            }
        }

        public string Translate(string key)
        {
            return TranslationProvider?.Translate(key) ?? string.Format("!{0}!", key);
        }

    }
}
