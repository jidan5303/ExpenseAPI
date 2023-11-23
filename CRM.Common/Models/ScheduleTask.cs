using CRM.Common.VM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
  public class ScheduleTask : BaseClass
  {
    public int ScheduleTaskID { get; set; }
    public string? TaskName { get; set; }
    public string? Description { get; set; }
    public int? PriorityLevel { get; set; }
    public DateTime? DueDate { get; set; }
    public int? AssigneeID { get; set; }
    public int? TaskStatus { get; set; }
    public int? RelatedTo { get; set; }
    public int? RelatedToID { get; set; }
    [NotMapped]
    public List<VMScheduleTaskTag> lstVMScheduleTaskTag { get; set; } = new List<VMScheduleTaskTag>();
  }
}
