using DemoWebAppMvc.Interface;
using DemoWebAppMvc.Models;
using DemoWebAppMvc.Repository;
using DemoWebAppMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebAppMvc.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleRepository _VehicleRepository;
        private readonly IPhotoServices _PhotoServices;
        private readonly IHttpContextAccessor _HttpContext;

        public VehicleController(IVehicleRepository vehicleRepository, IPhotoServices photoServices, IHttpContextAccessor httpContext) 
        {
           _VehicleRepository = vehicleRepository;
           _PhotoServices = photoServices;
           _HttpContext = httpContext;
        }
        public async Task<IActionResult> Index()
        {
            var curUser = _HttpContext.HttpContext.User.GetUserID();
            var vehlice = await _VehicleRepository.GetAllVehcilceByUserId(curUser);

            return View(vehlice);
        }
        public async Task<IActionResult> Create()
        {
            var curUser = _HttpContext.HttpContext?.User.GetUserID();
            var CreateNewVehicle = new CreateVehicleViewModel()
            {
                AppUserId = curUser
            };
            return View(CreateNewVehicle);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateVehicleViewModel vehicleModel)
        {
            if(ModelState.IsValid)
            {
                var ImageUpload = await _PhotoServices.AddPhotoAsync(vehicleModel.Image);
                var vehicle = new VehicleModel()
                {
                    AppUserId = vehicleModel.AppUserId,
                    Name = vehicleModel.Name,
                    Image = ImageUpload.Url.ToString(),
                    HorsePower = vehicleModel.HorsePower,
                    vehicleCategory = vehicleModel.vehicleCategory,

                };
                _VehicleRepository.Add(vehicle);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo not uploaded");
            }

            return View(vehicleModel);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var vehicle = await _VehicleRepository.GetVehcilceById(id);
            if (vehicle == null)
            {
                return View("Error");
            }
            var vehicleEdit = new EditVehicleViewModel 
            {
                Id = id,
                AppUserId = vehicle.AppUserId,
                Name = vehicle.Name,
                vehicleCategory= vehicle.vehicleCategory,
                HorsePower = vehicle.HorsePower,
                URL = vehicle.Image
                
            };
            return View(vehicleEdit);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditVehicleViewModel editVehicleView)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit vehicle");
                return View("Edit", editVehicleView);

            }

            var userVehicle = await _VehicleRepository.GetVehcilceByIdNoTracking(id);
            if (userVehicle != null)
            {
                try
                {
                    await _PhotoServices.DeletePhotoAsync(userVehicle.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "could not delete photo");
                    return View(editVehicleView);
                }
                var photoResult = await _PhotoServices.AddPhotoAsync(editVehicleView.Image);

                var vehicleModel = new VehicleModel
                {
                    Id = id,
                    Name = userVehicle.Name,
                    HorsePower = userVehicle.HorsePower,
                    AppUserId = userVehicle.AppUserId,
                    Image = photoResult.Url.ToString(),
                    vehicleCategory = userVehicle.vehicleCategory
                };
                _VehicleRepository.Update(vehicleModel);
                return RedirectToAction("Index");

            }
            else
            {
                return View(editVehicleView);
            }
        }
    }
}
