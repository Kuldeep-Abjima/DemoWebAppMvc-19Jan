using DemoWebAppMvc.Interface;
using DemoWebAppMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebAppMvc.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IUsersRepository _UsersRepository;
        private readonly IDashBoardRepository _DashBoardRepository;
        private readonly IHttpContextAccessor _HttpContext;

        public UserProfileController(IUsersRepository usersRepository, IDashBoardRepository dashBoardRepository, IHttpContextAccessor httpContext)
        {
            _UsersRepository = usersRepository;
            _DashBoardRepository = dashBoardRepository;
            _HttpContext = httpContext;
        }


        public async Task<IActionResult> Index()
        {
            var curUserID = _HttpContext.HttpContext.User.GetUserID();
             
            var curUser = await _UsersRepository.GetUsersById(curUserID);

            var UserProfileViewModel = new UserProfileViewModel()
            {
                UserName = curUser.UserName,
                ProfileImage = curUser.ProfileImage,
                Pace = curUser.Pace,
                Milage = curUser.Mileage,
                City = curUser.City,
                State = curUser.State,

            };
            return View(UserProfileViewModel);
        }
    }
}
