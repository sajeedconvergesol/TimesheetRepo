namespace TMS.API.DTOs
{
    public class InvoiceDetailRequestDTO
    {
        public int InvoiceId { get; set; }

        public int TaskAssignmentId { get; set; }

        public double HoursBilled { get; set; }

        public double RatePerHour { get; set; }
    }
}
