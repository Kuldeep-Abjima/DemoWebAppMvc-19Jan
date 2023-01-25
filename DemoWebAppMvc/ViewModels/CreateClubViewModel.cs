using DemoWebAppMvc.Data.ENum;
using DemoWebAppMvc.Models;

namespace DemoWebAppMvc.ViewModels
{
    public class CreateClubViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public ClubCategory ClubCategory { get; set; }
        public Address Address { get; set; }    

        public IFormFile Image { get; set; }

        public ClubCategory clubCategory { get; set; }


        public string AppUserId { get; set; }

    }
}
