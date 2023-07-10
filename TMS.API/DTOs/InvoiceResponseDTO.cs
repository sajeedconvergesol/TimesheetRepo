namespace TMS.API.DTOs
{
    public class InvoiceResponseDTO
    {
        public int Id { get; set; }
        public int TimeSheetMasterId { get; set; }
        public int CategoryId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double TotalAmount { get; set; }
        public IEnumerable<InvoiceDetailResponseDTO> InvoiceDetails { get; set; } = new List<InvoiceDetailResponseDTO>();
    }
}
