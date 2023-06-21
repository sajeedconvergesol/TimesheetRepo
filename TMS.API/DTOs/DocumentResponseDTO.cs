namespace TMS.API.DTOs
{
    public class DocumentResponseDTO
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentUrl { get; set; }
    }
}
