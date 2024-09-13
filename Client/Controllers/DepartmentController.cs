using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
