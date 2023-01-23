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

        public RaceController(AppDbContext context, IRaceRepository raceRepository, IPhotoServices configPhoto)
        {
            _context = context;
            _raceRepository = raceRepository;
            _configPhoto = configPhoto;
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
            if(race == null)
            {
                return View("Error");
            }
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
    }
}
