using DemoWebAppMvc.Data;
using DemoWebAppMvc.Interface;
using DemoWebAppMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoWebAppMvc.Repository
{
    public class RaceRepository : IRaceRepository
    {
            private readonly AppDbContext _context;

            public RaceRepository(AppDbContext context)
            {
                _context = context;
            }


            public bool Add(Race race)
            {
                _context.Add(race);
                return Save();
            }

            public bool Delete(Race race)
            {
                _context.Remove(race);
                return Save();
            }

            public async Task<IEnumerable<Race>> GetAll()
            {
                var race = _context.Races.ToListAsync();
                return await race;
            }

            public async Task<Race> GetByIdAsync(int id)
            {
                var race = await _context.Races.Include(c=> c.Address).FirstOrDefaultAsync(x => x.Id == id);
                return race;
            }
            public async Task<Race> GetByIdAsyncNoTracking(int id)
            {
            var race = await _context.Races.Include(c => c.Address).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return race;
            }

        public async Task<IEnumerable<Race>> GetRacesByCity(string city)
            {
                return await _context.Races.Where(x => x.Address.City.Contains(city)).ToListAsync();
            }

            public bool Save()
            {
                var saved = _context.SaveChanges();
                return saved > 0 ? true : false;
            }

            public bool Update(Race race)
            {
               _context.Update(race);  
               return Save();
            }

                
        }
    }
