using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.AspNet.Razor.TagHelpers;
using RestSharp.Extensions;

namespace WebUI.TagHelpers
{
    [HtmlTargetElement("avatar", Attributes = UserId)]
    public class AvatarTagHelper : TagHelper
    {
        private readonly IDAL _dal;

        public AvatarTagHelper(IDAL dal)
        {
            _dal = dal;
        }

        private const string UserId = "user-id";

        [HtmlAttributeName(UserId)]
        public string Id { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var htmlClass = context.AllAttributes["class"];
            string pathToAvatar = _dal.GetAvatarPathByUserId(UserId);
            output.Content.AppendHtml($"<img data-user-id={UserId} src=\"{pathToAvatar}\" ");

            if (htmlClass != null)
            {
                output.Content.AppendHtml($"class=\"{htmlClass.Value.ToString()}\"");
            }

            output.Content.AppendHtml("/>");

            base.Process(context, output);
        }
    }
}
