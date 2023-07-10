using System.Security.Cryptography;

namespace TMS.API.DTOs
{
    public class InvoiceDetailResponseDTO
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int TaskAssignmentId { get; set; }
        public double HoursBilled { get; set; }
        public double RatePerHour { get; set; }
    }
}
