using AspNetCoreWebIdentityApp.Repository.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Text.Encodings.Web;

namespace AspNetCoreWebIdentityApp.Web.TagHelpers
{
    public class UserRoleNamesTagHelper : TagHelper
    {
        public string UserId { get; set; } = null!;

        private readonly UserManager<AppUser> _userManager;

        public UserRoleNamesTagHelper(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "d-inline-flex flex-wrap gap-1");
            output.Attributes.RemoveAll("user-id");

            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                output.SuppressOutput();
                return;
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var stringBuilder = new StringBuilder();

            if (userRoles.Count == 0)
            {
                stringBuilder.Append("<span class='badge rounded-pill bg-light text-secondary border'>No role</span>");
            }
            else
            {
                foreach (var role in userRoles)
                {
                    stringBuilder.Append($"<span class='badge rounded-pill bg-success-subtle text-success-emphasis border border-success-subtle'>{HtmlEncoder.Default.Encode(role)}</span>");
                }
            }

            output.Content.SetHtmlContent(stringBuilder.ToString());
        }
    }
}