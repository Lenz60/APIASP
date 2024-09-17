using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.data = "ViewBagAuth";
            return View();
        }
    }
}
