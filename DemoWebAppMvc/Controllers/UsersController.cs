using DemoWebAppMvc.Interface;
using DemoWebAppMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebAppMvc.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersRepository _usersRepository;

        public UsersController(IUsersRepository usersRepository) 
        {
           _usersRepository = usersRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var Users = await _usersRepository.GetAllUsers();
            List<UsersViewModel> result = new List<UsersViewModel>();
            foreach (var user in Users)
            {
                var userViewModel = new UsersViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Pace = user.Pace,
                    Milage = user.Mileage,
                    ProfileImage = user.ProfileImage   
                   
                };
                result.Add(userViewModel);
            }
            
            return View(result);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var user = await _usersRepository.GetUsersById(id);
            var userViewModel = new UsersViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Pace = user.Pace,
                Milage = user.Mileage,
                ProfileImage = user.ProfileImage
            };

            return View(userViewModel);
        }
        
       
    }
}