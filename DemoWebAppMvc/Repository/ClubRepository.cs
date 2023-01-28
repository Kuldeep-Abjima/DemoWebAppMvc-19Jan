using DemoWebAppMvc.Data;
using DemoWebAppMvc.Interface;
using DemoWebAppMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoWebAppMvc.Repository
{
    public class ClubRepository : IClubRepository
    {
        private readonly AppDbContext _context;

        public ClubRepository(AppDbContext context)
        {
            _context = context;
        }


        public bool Add(Club club)
        {
           _context.Add(club);
            return Save();
        }

        public bool Delete(Club club)
        {
            _context.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetAll()
        {
           var club =  _context.Clubs.ToListAsync();
            return await club;
        }

        public async Task<Club> GetByIdAsync(int id)
        {
                var club = _context.Clubs.Include(c=>c.Address).FirstOrDefaultAsync(x => x.Id == id);
                return await club;
        }

        public async Task<Club> GetByIdAsyncNoTracking(int id)
        {
            var club = _context.Clubs.Include(c => c.Address).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return await club;
        }
        public async Task<IEnumerable<Club>> GetClubByCity(string city)
        {
               var result = await _context.Clubs.Where(x=>x.Address.City.Contains(city)).ToListAsync();
               return result;
        }

        public bool Save()
        {
                var saved = _context.SaveChanges();
                return saved > 0 ? true: false;
        }

        public bool Update(Club club)
        {
            _context.Update(club);
            return Save();
        }
    }
}
