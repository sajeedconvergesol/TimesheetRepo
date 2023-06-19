using System.ComponentModel.DataAnnotations;

namespace TMS.API.DTOs
{
    public class ChangePasswordDTO
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage ="Current Password field is requied")]
        [DataType(DataType.Password)]
       public string CurrentPassword { get; set; }
        [Required(ErrorMessage ="new Password field is requied")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirm Password field is requied")]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

    }
}
