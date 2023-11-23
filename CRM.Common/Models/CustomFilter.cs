using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
	public class CustomFilter : BaseClass
	{
		public int CustomFilterID { get; set; }

		public int? UserID { get; set; }

		public string? CustomFilterName { get; set; }

		public string? PageName { get; set; }

		public string? FilterObject { get; set; }

		public bool? IsDefault { get; set; }

	}

}
