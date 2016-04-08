using System.Collections.Generic;
using System.Globalization;

namespace WebUI.Services
{
    interface ITranslationProvider
    {
        IEnumerable<string> Languages { get; }
        string Translate(string key);
    }
}