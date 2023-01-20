using DemoWebAppMvc.Data;
using DemoWebAppMvc.Interface;
using DemoWebAppMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoWebAppMvc.Controllers
{
    public class ClubController : Controller
    {

        private readonly AppDbContext _Context;
        private readonly IClubRepository _clubRepository;

        public ClubController(AppDbContext context, IClubRepository clubRepository)
        {
            _Context = context;
            _clubRepository = clubRepository;
        }


        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubRepository.GetAll();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            return View(club);
        }


        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Club club)
        {
            if(!ModelState.IsValid)
            {
                return View(club);
            }
            _clubRepository.Add(club);
            return RedirectToAction("Index");
        }
    }
}
