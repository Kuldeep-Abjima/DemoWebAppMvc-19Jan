using DemoWebAppMvc.Data;
using DemoWebAppMvc.Data.ENum;
using DemoWebAppMvc.Interface;
using DemoWebAppMvc.Models;
using DemoWebAppMvc.Repository;
using DemoWebAppMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace DemoWebAppMvc.Controllers
{
    public class RaceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IRaceRepository _raceRepository;
        private readonly IPhotoServices _configPhoto;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RaceController(AppDbContext context, IRaceRepository raceRepository, IPhotoServices configPhoto, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _raceRepository = raceRepository;
            _configPhoto = configPhoto;
            _httpContextAccessor = httpContextAccessor;
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
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserID();
            var CreateRaceViewModel = new CreateRaceViewModel()
            {
                AppUserID = curUser
            };
            return View(CreateRaceViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceVm)
        {
            if (ModelState.IsValid)
            {
                var ImageUpload = await _configPhoto.AddPhotoAsync(raceVm.Image);
                var race = new Race
                {
                    Title = raceVm.Title,
                    Description = raceVm.Description,
                    Address = new Address
                    {
                        City = raceVm.Address.City,
                        Street = raceVm.Address.Street,
                        State = raceVm.Address.State,
                    },
                    AppUserId = raceVm.AppUserID,
                    RaceCategory = raceVm.RaceCategory,
                    Image = ImageUpload.Url.ToString()

                };
                _raceRepository.Add(race);
                return RedirectToAction("Index");

            }
            else
            {
                ModelState.AddModelError("", "Photo not uploaded");
            }

            return View(raceVm);


            //if (!ModelState.IsValid)
            //{
            //    return View(race);
            //}
            //_raceRepository.Add(race);
            //return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var race = await _raceRepository.GetByIdAsync(id);
            if (race == null || race.AppUser == null)
            {
                return View("Error");
            }
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserID();
            var curUserRole = _httpContextAccessor.HttpContext?.User.GetUserRole();
            if (race.AppUserId == curUser || curUserRole == "admin")
            {
                var raceVm = new EditRaceViewModel
                {
                    Title = race.Title,
                    Description = race.Description,
                    Address = race.Address,
                    AddressId = race.AddressId,
                    URL = race.Image,
                    RaceCategory = race.RaceCategory,
                };
                return View(raceVm);
            }
            else
            {
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel racevm)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit Race");
                return View("Edit", racevm);

            }

            var userRace = await _raceRepository.GetByIdAsyncNoTracking(id);
            if(userRace != null)
            {
                try
                {
                    await _configPhoto.DeletePhotoAsync(userRace.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "could not delete photo");
                    return View(racevm);
                }
                var photoResult = await _configPhoto.AddPhotoAsync(racevm.Image);

                var race = new Race
                {
                    Id= id,
                    Title = userRace.Title,
                    Description = userRace.Description,
                    Address = userRace.Address,
                    AddressId = userRace.AddressId,
                    Image = photoResult.Url.ToString(),
                    RaceCategory = userRace.RaceCategory

                };
                _raceRepository.Update(race);
                return RedirectToAction("Index");

            }
            else
            {
                return View(racevm);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var RaceDetails = await _raceRepository.GetByIdAsync(id);
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserID();
            var curUserRole = _httpContextAccessor.HttpContext?.User.GetUserRole();

            if (RaceDetails.AppUserId == curUser || curUserRole == "admin")
            {
                return View(RaceDetails);
               
            }
            else if(RaceDetails.AppUserId == null) 
            {
                if(curUser != null)
                {
                    if(curUserRole == "admin") 
                    {

                        return View(RaceDetails);
                    }
                    else
                    {
                        return View("Error");
                    }
                }
                else
                {
                    return View("Error");
                }

            }
            else
            {
                return View("Error");
            }
           
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var raceDetails = await _raceRepository.GetByIdAsync(id);
            if(raceDetails == null)
            {
                return View("Error");
            }
             _raceRepository.Delete(raceDetails);
            return RedirectToAction("Index","Race");
        }
    }
}
