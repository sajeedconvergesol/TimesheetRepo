namespace TMS.API.DTOs
{
    public class ProjectResponseDTO
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
