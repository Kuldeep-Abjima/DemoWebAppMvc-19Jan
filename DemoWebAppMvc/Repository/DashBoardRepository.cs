using DemoWebAppMvc.Data;
using DemoWebAppMvc.Interface;
using DemoWebAppMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;

namespace DemoWebAppMvc.Repository
{
    public class DashBoardRepository : IDashBoardRepository
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoServices _photoservices;
        public DashBoardRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor, IPhotoServices photoServices)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _photoservices= photoServices;
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

       public async Task<AppUser> GetUserById(string id)
       {
            return await _context.Users.FindAsync(id);
       }
        public async Task<AppUser> GetUserByIdNoTracking(string id)
        {
            return await _context.Users.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public bool Update(AppUser user)
        {
            _context.Users.Update(user);
            return Save();
        }


        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true: false;
        }
    }
}
