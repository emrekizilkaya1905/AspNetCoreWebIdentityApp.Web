using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWebIdentityApp.Web.Controllers
{
    public class OrderController : Controller
    {
        [Authorize(Policy = "OrderPermissionReadAndDelete")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
