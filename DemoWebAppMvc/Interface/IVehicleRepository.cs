using DemoWebAppMvc.Models;

namespace DemoWebAppMvc.Interface
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<VehicleModel>> GetAllVehcilceByUserId(string id);

        bool Add(VehicleModel vehicleModel);
        bool Update(VehicleModel vehicleModel);
        bool Delete(VehicleModel vehicleModel);
        bool Save();
    }
}
