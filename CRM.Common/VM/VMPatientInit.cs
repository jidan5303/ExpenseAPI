using CRM.Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.VM
{

	public class VMPatientInit
	{
		public List<PatientOrganization> lstOrganization { get; set; } = new List<PatientOrganization>();
		public List<PatientAssignTo> lstAssignTo { get; set; } = new List<PatientAssignTo>();
		public List<PatientSource> lstSource { get; set; } = new List<PatientSource>();
		public List<PatientStatus> lstStatus { get; set; } = new List<PatientStatus>();
		public List<PatientTags> lstTags { get; set; } = new List<PatientTags>();
	}

	public class PatientOrganization
	{
		public int OrganizationID { get; set; }
		public int? OfficeID { get; set; }
		public string? OrganizationName { get; set; }
		public int NewValue { get; set; } = 0;
		public int OldValue { get; set; } = 0;
	}
	public class PatientAssignTo
	{
		
		public int? AssignID { get; set; }
		public string? AssignName { get; set; }
		public int TotalAssignValue { get; set; } = 0;

	}
	public class PatientSource
	{
		public string? SourceName { get; set; }
		public int TotalSourceValue { get; set; } = 0;
	}

	public class PatientTags
	{
		public string? TagsName { get; set; }
		public string? ColorCode { get; set; }
		public int TotalTagsValue { get; set; } = 0;

	}

	public class PatientStatus
	{
		public int? StatusID { get; set; }
		public string? StatusName { get; set; }
		public string? ColorCode { get; set; }
		public int TotalSatusValue { get; set; } = 0;

	}
}
