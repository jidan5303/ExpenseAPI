using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Common.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        [ForeignKey("Designation")]
        public int DesignationId { get; set; }
        public Designation? Designation { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        [ForeignKey("Supervisor")]
        public Guid? SupervisorId { get; set; }
        public Employee? Supervisor { get; set; }

        public DateTime? JoiningDate { get; set; }
        public string? Address { get; set; }
        public List<Attachment>? Attachments { get; set; }
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("CreatedBy")]
        public Guid? CreatedById { get; set; }
        public Employee? CreatedBy { get; set; }

    }
}
