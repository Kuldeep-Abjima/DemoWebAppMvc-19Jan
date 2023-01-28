using DemoWebAppMvc.Models;
using System.Collections;

namespace DemoWebAppMvc.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Club> Clubs { get; set; }    

        public IEnumerable<Race> Races { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
