using System.ComponentModel.DataAnnotations;
using TMS.Core;

namespace TMS.API.DTOs
{
    public class ProjectDocRequestDTO
    {
        public int ProjectId { get; set; }
        public string DocumentName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public IFormFile File { get; set; }
    }
}
