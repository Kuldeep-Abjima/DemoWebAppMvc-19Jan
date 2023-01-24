using System.ComponentModel.DataAnnotations;

namespace DemoWebAppMvc.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email Address is Required")]
        public string EmailAddress { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string  Password { get; set; }
       
        
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [Compare("Password", ErrorMessage = "Password does'nt Match")]
        public string ConfirmPassword { get; set; }
    }
}
