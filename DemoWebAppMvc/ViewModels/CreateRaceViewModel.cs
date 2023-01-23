using DemoWebAppMvc.Data.ENum;
using DemoWebAppMvc.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoWebAppMvc.ViewModels
{
    public class CreateRaceViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public IFormFile Image { get; set; }

        public Address Address { get; set; }

        public RaceCategory RaceCategory { get; set; }
    }
}
