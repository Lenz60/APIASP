using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
