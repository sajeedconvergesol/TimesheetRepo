using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Core
{
    public class TimeSheetDetails
    {
        [Key]
        public int Id { get; set; }
        public int TimeSheetMasterId { get; set; }
        public int TaskAssignmentId { get; set; }
        public string Period { get; set; }
        public int Hours { get; set; }
    }
}
