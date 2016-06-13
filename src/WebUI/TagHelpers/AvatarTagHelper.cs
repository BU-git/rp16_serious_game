using System.Threading.Tasks;
using Interfaces;
using Microsoft.AspNet.Razor.TagHelpers;

namespace WebUI.TagHelpers
{
    [HtmlTargetElement("avatar", TagStructure = TagStructure.WithoutEndTag)]
    public class AvatarTagHelper : TagHelper
    {
        private readonly IDAL _dal;

        public AvatarTagHelper(IDAL dal)
        {
            _dal = dal;
        }

        public string UserId { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var htmlClass = context.AllAttributes["class"];
            output.TagName = "img";
            string pathToAvatar = await _dal.GetAvatarPathByUserId(UserId);
            output.Attributes.Add("data-user-id", UserId);
            output.Attributes.Add("src", pathToAvatar.Remove(0,1));
            output.Attributes.Add("asp-append-version", true);
            if (htmlClass != null)
            {
                output.Attributes.Add("class", htmlClass.Value.ToString());
            }
        }
    }
}
