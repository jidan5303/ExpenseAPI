using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
	public class PatientAndTagMapping
	{
		public int PatientAndTagMappingID { get; set; }
		public int? PatientID { get; set; }
		public int? TagID { get; set; }
		public int? Status { get; set; }
	}
}
