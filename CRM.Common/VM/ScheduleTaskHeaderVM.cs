using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.VM
{
	public class ScheduleTaskHeaderVM
	{
		public int? Priority { get; set; }
		public int? DueToDay { get; set; }
		public int? PassDue { get; set; }
		public int? Upcomming { get; set; }
	}
}
