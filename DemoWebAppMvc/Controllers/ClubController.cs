using DemoWebAppMvc.Data;
using DemoWebAppMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebAppMvc.Controllers
{
    public class ClubController : Controller
    {

        private readonly AppDbContext _Context;

        public ClubController(AppDbContext context)
        {
            _Context = context;
        }


        public IActionResult Index()
        {
            List<Club> clubs = _Context.Clubs.ToList();
            return View(clubs);
        }
    }
}
