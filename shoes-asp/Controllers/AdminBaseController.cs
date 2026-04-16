using Microsoft.AspNetCore.Mvc;

namespace shoes_asp.Controllers
{
    public class AdminBaseController : Controller
    {
        protected bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == "Admin";
        }

        protected IActionResult CheckAdminAccess()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("UserRole");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (role != "Admin")
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            return null!;
        }
    }
}