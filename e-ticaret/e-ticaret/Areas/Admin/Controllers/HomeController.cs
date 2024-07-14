using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_ticaret.Areas.Admin.Controllers
{
    [Area("Admin")]

    //[Authorize(Roles ="Admin")]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
