using Microsoft.AspNetCore.Mvc;

namespace DemoWebAppMvc.Controllers
{
    public class RaceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
