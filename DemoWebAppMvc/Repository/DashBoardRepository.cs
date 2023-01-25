using DemoWebAppMvc.Data;
using DemoWebAppMvc.Interface;
using DemoWebAppMvc.Models;
using Microsoft.VisualBasic;

namespace DemoWebAppMvc.Repository
{
    public class DashBoardRepository : IDashBoardRepository
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashBoardRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Club>> GetAllUserClubs()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserID();
            var userClub =   _context.Clubs.Where(x => x.AppUser.Id == curUser);
            return userClub.ToList();
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserID();
            var userRace = _context.Races.Where(x => x.AppUser.Id == curUser);
            return userRace.ToList();
        }
    }
}
