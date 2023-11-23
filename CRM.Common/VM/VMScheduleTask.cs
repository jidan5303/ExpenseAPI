using CRM.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.VM
{
	public class VMScheduleTask
	{
		public int ScheduleTaskID { get; set; }
		public string? TaskName { get; set; }
		public string? Description { get; set; }
		public int? PriorityLevel { get; set; }
		public DateTime? DueDate { get; set; }
		public int? AssigneeID { get; set; }
		public string? AssigneeName { get; set; }
		public int? TaskStatus { get; set; }
		public int? RelatedTo { get; set; }
		public int? RelatedToID { get; set; }
		public string? RelatedToName { get; set; }
		public int? RelatedToGender { get; set; }

		[NotMapped]
		public List<VMScheduleTaskTag>? lstVMScheduleTaskTag { get; set; }
	}

	public class VMScheduleTaskTag
	{
		public int ScheduleTaskAndTagID { get; set; }
		public int ScheduleTaskID { get; set; }
		public int TagID { get; set; }
		public int Status { get; set; }
		public string TagTitle { get; set; }
		public string Color { get; set; }
	}

}
