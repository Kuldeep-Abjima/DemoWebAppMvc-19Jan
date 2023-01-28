using DemoWebAppMvc.Data.ENum;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoWebAppMvc.ViewModels
{
    public class CreateVehicleViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }


        public int? HorsePower { get; set; }

        public VehicleCategory vehicleCategory { get; set; }

        public IFormFile Image { get; set; }

        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }


    }
}
