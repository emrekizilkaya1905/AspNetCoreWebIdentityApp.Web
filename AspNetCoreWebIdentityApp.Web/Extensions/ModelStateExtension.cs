using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCoreWebIdentityApp.Web.Extensions
{
    public static class ModelStateExtension
    {
        // Bu extension method sayesinde, ModelState.AddModelError() methodunu tek tek cagirmak yerine, bir listeyi tek seferde ekleyebiliriz.
        public static void AddModelErrorList(this ModelStateDictionary modelState, List<string> errors)
        {
           errors.ForEach(error => modelState.AddModelError(string.Empty, error));
        }
    }
}
