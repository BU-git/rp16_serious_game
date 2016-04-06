using System.Collections.Generic;
using System.Globalization;

namespace WebUI.Services
{
    interface ITranslationProvider
    {
        IEnumerable<CultureInfo> Languages { get; }
        object Translate(string key);
    }
}