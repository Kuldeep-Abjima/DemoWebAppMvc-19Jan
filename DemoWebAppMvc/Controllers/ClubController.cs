using DemoWebAppMvc.Data;
using DemoWebAppMvc.Interface;
using DemoWebAppMvc.Models;
using DemoWebAppMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoWebAppMvc.Controllers
{
    public class ClubController : Controller
    {

        private readonly AppDbContext _Context;
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoServices _photoServices;

        public ClubController(AppDbContext context, IClubRepository clubRepository, IPhotoServices photoServices)
        {
            _Context = context;
            _clubRepository = clubRepository;
            _photoServices = photoServices;
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
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                var result =  await _photoServices.AddPhotoAsync(clubVM.Image);
                var club = new Club
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Address = new Address
                    {
                        City = clubVM.Address.City,
                        Street = clubVM.Address.Street,
                        State = clubVM.Address.State,
                    },
                    clubCategory = clubVM.ClubCategory,
                    Image = result.Url.ToString()

                };
                _clubRepository.Add(club);
                return RedirectToAction("Index");

            }
            else
            {
                ModelState.AddModelError("", "PhotoUploadFailed");
            }
            return View(clubVM);
            //if(!ModelState.IsValid)
            //{
            //    return View(club);
            //}
            //_clubRepository.Add(club);
            //return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            if(club == null)
            {
                return View("Error");
            }

            var ClubVm = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                URL = club.Image,
                AddressId = club.AddressId,
                Address = club.Address,
                ClubCategory = club.clubCategory

            };
            return View(ClubVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVm)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubVm);
            }

            var userClub = await _clubRepository.GetByIdAsyncNoTracking(id);
            if(userClub != null)
            { 
                try
                {
                    await _photoServices.DeletePhotoAsync(userClub.Image);

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "could not delete photo");
                    return View(clubVm);
                }

                var photoResult = await _photoServices.AddPhotoAsync(clubVm.Image);

                var club = new Club
                {
                    Id = id,
                    Title = clubVm.Title,
                    Description = clubVm.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = clubVm.AddressId,
                    Address = clubVm.Address,
                };

            _clubRepository.Update(club);
            
            return RedirectToAction("Index");

            }
            else
            {
                return View(clubVm);
            }


        }

    }
}
