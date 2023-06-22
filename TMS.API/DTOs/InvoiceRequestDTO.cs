namespace TMS.API.DTOs
{
    public class InvoiceRequestDTO
    {
        public int TimeSheetMasterId { get; set; }

        public int CategoryId { get; set; }

        public DateTime InvoiceDate { get; set; }

        public double TotalAmount { get; set; }
        public List<InvoiceDetailRequestDTO> InvoiceDetailRequestDTO { get; set; }
    }
}
