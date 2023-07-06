using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Core
{
    public class InvoiceDetails
    {
        protected int _Id, _InvoiceId, _TaskAssignmentId;
        protected double _HoursBilled, _RatePerHour;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get => _Id;
            set => _Id = value;
        }
        public int InvoiceId
        {
            get => _InvoiceId;
            set => _InvoiceId = value;
        }
        public int TaskAssignmentId
        {
            get => _TaskAssignmentId;
            set => _TaskAssignmentId = value;
        }
        public double HoursBilled
        {
            get => _HoursBilled;
            set => _HoursBilled = value;
        }
        public double RatePerHour
        {
            get => _RatePerHour;
            set => _RatePerHour = value;
        }
    }
}
