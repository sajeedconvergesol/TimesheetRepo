namespace TMS.API.DTOs
{
    public class TimeSheetDetailRequestDTO
    {
        public int Id { get; set; }
        public int TimeSheetMasterId { get; set; }
        public int TaskAssignmentId { get; set; }
        public string Period { get; set; }
        public int Hours { get; set; }
    }
}
