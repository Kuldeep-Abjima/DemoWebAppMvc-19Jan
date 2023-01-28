using DemoWebAppMvc.Helper;
using DemoWebAppMvc.Interface;
using DemoWebAppMvc.Models;
using DemoWebAppMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;

namespace DemoWebAppMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _Configuration;
        private readonly IClubRepository _clubRepository;
        private readonly IRaceRepository _raceRepository;

        public HomeController(ILogger<HomeController> logger,IConfiguration configuration,IClubRepository clubRepository, IRaceRepository raceRepository)
        {
            _logger = logger;
            _Configuration = configuration;
            _clubRepository = clubRepository;
            _raceRepository = raceRepository;
        }

        public async Task<IActionResult> Index()
        {
            var ipinfo = new IPinfo();
            var homeViewModel = new HomeViewModel();
            try
            {
                string url = _Configuration.GetValue<string>("SystemUrl");
                var info = new WebClient().DownloadString(url);
                ipinfo = JsonConvert.DeserializeObject<IPinfo>(info);
                RegionInfo myRI1 = new RegionInfo(ipinfo.Country);
                homeViewModel.City = ipinfo.City;
                homeViewModel.State = ipinfo.Region;
                if(homeViewModel.City != null)
                {
                     homeViewModel.Clubs = await _clubRepository.GetClubByCity(homeViewModel.City);
                     homeViewModel.Races = await _raceRepository.GetRacesByCity(homeViewModel.City);
                    if (homeViewModel.Clubs != null && homeViewModel.Clubs.Count() > 0)
                     {
                            return View(homeViewModel);
                     }
                    else
                    {
                        homeViewModel.Clubs = null;
                    }
                }
                else
                {
                    homeViewModel.Clubs = null;
                    homeViewModel.Races = null;
                }
                return View(homeViewModel);
            }
            catch(Exception ex)
            {
                homeViewModel.Clubs = null;
                homeViewModel.Races = null;
            }
            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}