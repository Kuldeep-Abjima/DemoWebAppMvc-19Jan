using DemoWebAppMvc.Data;
using DemoWebAppMvc.Interface;
using DemoWebAppMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoWebAppMvc.Repository
{
    public class UserRepository : IUsersRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) 
        {
           _context = context;
        }

        public bool Add(AppUser user)
        {
            throw new NotImplementedException();
        }

        public bool Delete(AppUser user)
        {
                _context.Users.Remove(user);
                return Save();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<AppUser> GetUsersById(string id)
        {
            var user = await _context.Users.FindAsync(id);
            return user;
        }

        public bool Save()
        {
                var Saved = _context.SaveChanges();
                return Saved > 0 ? true : false;
        }

        public bool Update(AppUser user)
        {    
              _context.Update(user);
              return Save();
        }
    }
}
