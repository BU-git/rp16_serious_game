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
        private static volatile TranslationManager manager;
        private static object syncRoot = new Object();

        
        public static TranslationManager Instance
        {
            get
            {
                if (manager == null)
                {
                    lock (syncRoot)
                    {
                        if (manager == null)
                            manager = new TranslationManager();
                    }
                }

                return manager;
            }
        }

        private TranslationManager()
        {
           
        }

        public ITranslationProvider TranslationProvider { get; set; }
       

        public IEnumerable<CultureInfo> Languages
        {
            get
            {
                return TranslationProvider != null ? TranslationProvider.Languages : Enumerable.Empty<CultureInfo>();
            }
        }

        private CultureInfo currentLanguage;
        public CultureInfo CurrentLanguage
        {
            get
            {
                return currentLanguage ?? (currentLanguage = new CultureInfo("en"));
            }
            set
            {
                if (value != currentLanguage)
                {
                    currentLanguage = value;

                }
            }
        }

        public object Translate(string key)
        {
            if (TranslationProvider != null)
            {
                object translation = TranslationProvider.Translate(key);
                if (translation != null)
                    return translation;
            }
            return string.Format("!{0}!", key);
        }

    }
}
