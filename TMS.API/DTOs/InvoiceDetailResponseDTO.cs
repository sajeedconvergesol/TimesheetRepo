using System.Security.Cryptography;

namespace TMS.API.DTOs
{
    public class InvoiceDetailResponseDTO
    {
        public long Id { get; set; }
        public long InvoiceId { get; set; }
        public long TaskAssignmentId { get; set; }
        public double HoursBilled { get; set; }
        public double RatePerHour { get; set; }
    }
}
