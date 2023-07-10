using System.ComponentModel.DataAnnotations;

namespace TMS.API.DTOs
{
    public class PostUserDTO
    {
        [Required(ErrorMessage ="Username Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "FirstName Required")]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required(ErrorMessage = "MobileNumber Required")]
        public string MobileNo { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Gender { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }
        public int ManagerId { get; set; }
    }
}
