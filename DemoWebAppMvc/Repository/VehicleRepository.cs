using DemoWebAppMvc.Data;
using DemoWebAppMvc.Interface;
using DemoWebAppMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoWebAppMvc.Repository
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _Context;
        private readonly IPhotoServices _PhotoServices;

        public VehicleRepository(AppDbContext context, IPhotoServices photoServices)
        {
            _Context = context;
            _PhotoServices = photoServices;
        }

        public bool Add(VehicleModel vehicleModel)
        {
            _Context.Add(vehicleModel);
            return Save();
        }

        public bool Delete(VehicleModel vehicleModel)
        {
            _Context.Remove(vehicleModel);
            return Save();
        }

        public async Task<IEnumerable<VehicleModel>> GetAllVehcilceByUserId(string id)
        {
                var vehicles =  await _Context.Vehicles.Where(x=>x.AppUserId == id).ToListAsync();
                return vehicles;
        }

        public bool Save()
        {
            var saved = _Context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(VehicleModel vehicleModel)
        {
            _Context.Update(vehicleModel);
            return Save();
        }
    }
}
