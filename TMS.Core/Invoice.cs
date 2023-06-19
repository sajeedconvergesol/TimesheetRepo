using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Core
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        public int TimeSheetMasterId { get; set; }
        public int CategoryId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public float TotalAmount { get; set; }
    }
}
