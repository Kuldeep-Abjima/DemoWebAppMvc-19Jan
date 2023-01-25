using DemoWebAppMvc.Data;
using DemoWebAppMvc.Interface;
using DemoWebAppMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebAppMvc.Controllers
{
    public class DashBoardController : Controller
    {

        private readonly IDashBoardRepository _dashBoardRepository;

        public DashBoardController(IDashBoardRepository dashBoardRepository)
        {
            _dashBoardRepository = dashBoardRepository;
        }
        public async Task<IActionResult> Index()
        {
            var userRaces = await _dashBoardRepository.GetAllUserRaces();
            var userClub = await   _dashBoardRepository.GetAllUserClubs();
            var dashboardViewModel = new DashBoardViewModel()
            {
                Races = userRaces,
                clubs = userClub
            };
            return View(dashboardViewModel);
        }
    }
}
