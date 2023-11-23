using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
	public class ScheduleTaskAndTagMapping
	{
		public int ScheduleTaskAndTagID { get; set; }
		public int ScheduleTaskID { get; set; }
		public int TagID { get; set; }
		public int Status { get; set; }
	}
}
