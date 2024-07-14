using Microsoft.AspNetCore.Mvc;

namespace e_ticaret.Controllers
{
    public class AccessController : Controller
    {
        public IActionResult Denied()
        {
            return View();
        }
    }
}
