using TMS.Core;

namespace TMS.API.DTOs
{
    public class TimesheetMasterResponseDTO
    {
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TotalHours { get; set; }
        public int TimeSheetStatus { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public int LastModifiedBy { get; set; }
        public IEnumerable<TimeSheetDetails> TimeSheetDetails { get; set; }
    }
}
