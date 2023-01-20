using DemoWebAppMvc.Data;
using DemoWebAppMvc.Interface;
using DemoWebAppMvc.Models;
using DemoWebAppMvc.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoWebAppMvc.Controllers
{
    public class RaceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IRaceRepository _raceRepository;
        public RaceController(AppDbContext context, IRaceRepository raceRepository)
        {
            _context = context;
            _raceRepository = raceRepository;
        }

     

        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> race = await _raceRepository.GetAll();
            return View(race);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var race = await _raceRepository.GetByIdAsync(id);
            return View(race);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Race race)
        {
            if (!ModelState.IsValid)
            {
                return View(race);
            }
            _raceRepository.Add(race);
            return RedirectToAction("Index");
        }
    }
}
