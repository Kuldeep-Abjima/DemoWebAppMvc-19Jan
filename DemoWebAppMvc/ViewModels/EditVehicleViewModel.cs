using DemoWebAppMvc.Data.ENum;
using DemoWebAppMvc.Models;

namespace DemoWebAppMvc.ViewModels
{
    public class EditVehicleViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public int? HorsePower { get; set; }

        public IFormFile Image { get; set; }

        public string? URL { get; set; }

        public VehicleCategory vehicleCategory { get; set; }


        public string? AppUserId { get; set; }

    }
}
