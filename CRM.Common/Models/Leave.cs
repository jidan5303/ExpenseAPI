using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Common.Models
{
    public class Leave
    {
        private Leave()
        {
        }
        public Leave(DateTime appliedDate, DurationType durationType, DateTime fromDate, DateTime toDate, Employee employee, LeaveType leaveType, string reason)
        {
            AppliedDate = appliedDate;
            DurationType = durationType;
            FromDate = fromDate;
            ToDate = toDate;
            Employee = employee;
            LeaveType = leaveType;
            Reason = reason;
        }


        [Key]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime AppliedDate { get; set; }

        public DurationType DurationType { get; set; }
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }

        [ForeignKey("Employee"), StringLength(36)]
        public Guid? ApprovedBy { get; set; }
        public Employee Employee { get; set; }

        [ForeignKey("LeaveType")]
        public int LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; }
        public string Reason { get; set; }
    }


    public enum DurationType
    {
        FullDay = 1,
        HalfDay = 2
    }



    public class LeaveType
    {
        private LeaveType()
        {

        }
        public LeaveType(string title)
        {
            Title = title;
        }

        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
