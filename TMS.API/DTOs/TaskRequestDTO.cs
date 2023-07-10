namespace TMS.API.DTOs
{
    public class TaskRequestDTO
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public int EstimatedHours { get; set; }
    }
}
