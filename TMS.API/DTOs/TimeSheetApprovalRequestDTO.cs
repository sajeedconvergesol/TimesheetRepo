namespace TMS.API.DTOs
{
    public class TimeSheetApprovalRequestDTO
    {
        public int Id { get; set; }
        public int TimeSheetMasterId { get; set; }
        public int ApprovalStatus { get; set; }
        public int ApprovalStatusBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
