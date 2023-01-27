using CloudinaryDotNet.Actions;
using DemoWebAppMvc.Data;
using DemoWebAppMvc.Interface;
using DemoWebAppMvc.Models;
using DemoWebAppMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DemoWebAppMvc.Controllers
{
    public class DashBoardController : Controller
    {

        private readonly IDashBoardRepository _dashBoardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoServices _photoServices;

        public DashBoardController(IDashBoardRepository dashBoardRepository, IHttpContextAccessor httpContextAccessor, IPhotoServices photoServices)
        {
            _dashBoardRepository = dashBoardRepository;
            _httpContextAccessor = httpContextAccessor;
            _photoServices = photoServices;
       
        }

        private void MapUserEdit(AppUser user, EditUserDashBoardViewModel editVM, ImageUploadResult photoResult)
        {
           user.Id = editVM.Id;
           user.Pace = editVM.Pace;
           user.Mileage = editVM.Mileage;
           user.ProfileImage = photoResult.Url.ToString();
           user.City = editVM.City;
           user.State= editVM.State;
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
        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserID();
            var user = await _dashBoardRepository.GetUserById(curUserId);
            if (user == null) 
            {
                return View("Error");
            }
            var editUserViewModel = new EditUserDashBoardViewModel()
            {
                Id = curUserId,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImage = user.ProfileImage,
                City = user.City,
                State = user.State,

            };
            return View(editUserViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashBoardViewModel editVm)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit Profile");
                return View("EditUserProfile",editVm);
            }

            var user = await _dashBoardRepository.GetUserByIdNoTracking(editVm.Id);
            if(user.ProfileImage == "" || user.ProfileImage == null)
            {
                var photoResult = await _photoServices.AddPhotoAsync(editVm.Image);

                MapUserEdit(user, editVm, photoResult);

                _dashBoardRepository.Update(user);
            
                return RedirectToAction("Index");
            }
            else
            {
                try
                {
                    await _photoServices.DeletePhotoAsync(editVm.Id);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "cou;d not delete photo");
                    return View(editVm);
                }
                var photoResult = await _photoServices.AddPhotoAsync(editVm.Image);
                MapUserEdit(user, editVm, photoResult);

                _dashBoardRepository.Update(user);

                return RedirectToAction("Index");
            }
             
        }

    }
}
