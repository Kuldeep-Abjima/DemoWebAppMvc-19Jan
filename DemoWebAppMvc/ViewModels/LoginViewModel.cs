using System.ComponentModel.DataAnnotations;

namespace DemoWebAppMvc.ViewModels
{
    public class LoginViewModel
    {

        [Display(Name = "EmailAddress")]
        [Required(ErrorMessage = "Email Address is Required")]
        public string EmailAddress { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string  Password { get; set; }
    
    
    }
}
