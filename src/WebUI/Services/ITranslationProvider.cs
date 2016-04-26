using System.Collections.Generic;

namespace WebUI.Services
{
    interface ITranslationProvider
    {
        IEnumerable<string> Languages { get; }
        string Translate(string key);
    }
}