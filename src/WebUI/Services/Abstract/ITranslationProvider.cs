using System.Collections.Generic;

namespace WebUI.Services.Abstract
{
    interface ITranslationProvider
    {
        IEnumerable<string> Languages { get; }
        string Translate(string key);
    }
}