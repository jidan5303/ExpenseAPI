using CRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.VM
{
	public class VMScheduleTaskNewInitData
	{
		public List<SystemUser>? Assignees { get; set; }
		public List<Tag>? Tags { get; set; }
		public List<VMScheduleTaskStatus>? TaskStatuses { get; set; }
	}

	public class VMScheduleTaskStatus
	{
		public int ScheduleTaskStatusID { get; set; }
		public string? StatusName { get; set; }
		public string? ColorCode { get; set; }
	}
}
