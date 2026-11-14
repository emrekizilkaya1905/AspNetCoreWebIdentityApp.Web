using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspNetCoreWebIdentityApp.Web.TagHelpers
{
    public class UserPictureThumbnailTagHelper : TagHelper
    {
        public string? PictureUrl { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";
            output.TagMode = TagMode.SelfClosing;

            if (string.IsNullOrWhiteSpace(PictureUrl))
            {
                output.Attributes.SetAttribute("src", "/userpictures/default_user_picture.png");
            }
            else
            {
                output.Attributes.SetAttribute("src", $"/userpictures/{PictureUrl}");
            }

            output.Attributes.SetAttribute("alt", "Profile picture");
            output.Attributes.SetAttribute("width", "64");
            output.Attributes.SetAttribute("height", "64");
            output.Attributes.SetAttribute("class", "rounded-circle border");
            output.Attributes.SetAttribute("style", "object-fit: cover;");
        }
    }
}
