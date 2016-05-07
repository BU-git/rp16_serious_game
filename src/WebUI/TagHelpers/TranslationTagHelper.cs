using Microsoft.AspNet.Razor.TagHelpers;
using WebUI.Services.Abstract;
using WebUI.Services.Concrete;

namespace WebUI.TagHelpers
{
    [HtmlTargetElement("translation", Attributes = TranslationAttributeName)]
    public class TranslationTagHelper : TagHelper
    {
        private readonly ITranslationProvider _translationProvider;

        public TranslationTagHelper()
        {
            _translationProvider = new JsonTranslationProvider(@".\Assets\json\translation.json");
        }

        private const string TranslationAttributeName = "trl-key";

        [HtmlAttributeName(TranslationAttributeName)]
        public string Key { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string value = _translationProvider.Translate(Key);
            output.Content.AppendHtml($"<span data-trl-key=\"{Key}\">{value}</span>");

            base.Process(context, output);
        }
    }
}
