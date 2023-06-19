using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Core
{
    public class ProjectDocuments
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string DocumentName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
