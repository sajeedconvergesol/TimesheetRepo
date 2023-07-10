namespace TMS.API.DTOs
{
    public class TaskAssignmentRequestDTO
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int EmployeeId { get; set; }
        public int TaskId { get; set; }
        public int TaskStatus { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
    }
}
