﻿namespace DemoWebAppMvc.ViewModels
{
    public class EditUserDashBoardViewModel
    {
        public string Id { get; set; }
        public int? Pace { get; set; }
        public int? Mileage { get; set; }

        public string? ProfileImage { get; set; }


        public string City { get; set; }

        public string State { get; set; }

        public IFormFile Image { get; set; }
    }
}
