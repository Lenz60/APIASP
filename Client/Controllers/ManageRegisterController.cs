using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class ManageRegisterController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.data = "ViewBagAuth";
            return View();
        }
    }
}
