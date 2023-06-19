using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Core
{
    public class ApplicationUser : IdentityUser<int>
    {
        //Extended Properties		
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsActive { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public string? MobileNo { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? ManagerId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
