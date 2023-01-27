using DemoWebAppMvc.Data;
using DemoWebAppMvc.Interface;
using DemoWebAppMvc.Models;
using DemoWebAppMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DemoWebAppMvc.Controllers
{
    public class ClubController : Controller
    {

        private readonly AppDbContext _Context;
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoServices _photoServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ClubController(AppDbContext context, IClubRepository clubRepository, IPhotoServices photoServices, IHttpContextAccessor httpContextAccessor)
        {
            _Context = context;
            _clubRepository = clubRepository;
            _photoServices = photoServices;
            _httpContextAccessor = httpContextAccessor;
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
            //var curUser = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserID();
            var CreateClubViewModel = new CreateClubViewModel() { AppUserId = curUserId };
            return View(CreateClubViewModel);
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
                    AppUserId = clubVM.AppUserId,
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
        public async Task<IActionResult> Delete(int id)
        {
            var ClubDetails = await _clubRepository.GetByIdAsync(id);
            if (ClubDetails == null)
            {
                return View("Error");
            }
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserID();
            var curUserRole = _httpContextAccessor.HttpContext?.User.GetUserRole();

            if (ClubDetails.AppUserId == curUser || curUserRole == "admin")
            {
                return View(ClubDetails);

            }
            else if (ClubDetails.AppUserId == null)
            {
                if (curUser != null)
                {
                    if (curUserRole == "admin")
                    {

                        return View(ClubDetails);
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
            var clubDetails = await _clubRepository.GetByIdAsync(id);
            if(clubDetails == null)
            {
                return View("Error");
            }
            _clubRepository.Delete(clubDetails);
            return RedirectToAction("Index");
        }

    }
}
