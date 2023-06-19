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
        public string CountryCode { get; set; }
        [MinLength(10),MaxLength(10)]
        public string PhoneNumber { get; set; }
        [MinLength(10), MaxLength(10)]
        [Required(ErrorMessage = "MobileNumber Required")]
        public string MobileNumber { get; set; }
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
