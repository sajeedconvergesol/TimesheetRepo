namespace TMS.API.DTOs
{
    public class RequestUserDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsActive { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public string? MobileNo { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? ManagerId { get; set; }
        public int? CreatedBy { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
