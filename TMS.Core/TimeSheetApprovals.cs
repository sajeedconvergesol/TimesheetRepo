using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Core
{
    public class TimeSheetApprovals
    {
        [Key]
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
