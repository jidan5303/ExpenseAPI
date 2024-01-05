using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class LeaveRequest
    {
        public int LeaveRequestID { get; set; }
        public int LeaveTypeID { get; set; }
        public int EmployeeID { get; set; }
        public decimal Duration { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Comment { get; set; }
        public string? LeaveStatus { get; set; }
        public string? RejectionCause { get; set; }
        public int Status { get; set; }
        [NotMapped]
        public LeaveEmployee? LeaveEmployee { get; set; }
        [NotMapped]
        public LeaveType? LeaveType { get; set; }
    }
}
