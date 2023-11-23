using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.VM
{
	public class VMPatientTag
	{
		public int PatientAndTagMappingID { get; set; }
		public int? PatientID { get; set; }
		public int? TagID { get; set; }
		public int? Status { get; set; }
		public string? TagTitle { get; set; }
		public string? Color { get; set; }
	}
}
