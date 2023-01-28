using DemoWebAppMvc.Interface;
using DemoWebAppMvc.Models;
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
                    Name = vehicleModel.Name,
                    Image = ImageUpload.Url.ToString(),
                    HorsePower = vehicleModel.HorsePower,
                    vehicleCategory = vehicleModel.vehicleCategory,

                };
                
            }
        }
    }
}
