using System.Drawing;
using TMS.Core;

namespace TMS.API.DTOs
{
    public class ListOfProjectDocResponseDTO
    {
        public int ProjectId { get; set; }
        public string DocumentName { get; set; }
        List<ProjectDocuments> projectDocuments { get; set; }
    }
}
