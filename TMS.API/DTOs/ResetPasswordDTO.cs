using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace TMS.API.DTOs
{
    public class ResetPasswordDTO
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage ="Email is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Code Is Required")]
        public string Code { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Password is required")]
        public string NewPassword { get; set; }
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
