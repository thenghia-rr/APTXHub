using Microsoft.AspNetCore.Mvc;

namespace APTXHub.Controllers.Base
{
    public class BaseController : Controller
    {
        protected IActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", "Authentication");
        }
    }
}
