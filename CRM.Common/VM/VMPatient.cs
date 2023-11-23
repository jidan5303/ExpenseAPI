using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.VM
{
    public class VMPatient
    {
        public List<VmPatient> lstPaitent { get; set; } = new List<VmPatient>();
        public List<VMPatientLastChange> lstPatientHistory { get; set; } = new List<VMPatientLastChange>();
    }
    public class VmPatient
    {
		public int PatientID { get; set; }
		public int? DepartmentID { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Email { get; set; }
		public string? HomePhone { get; set; }
		public string? CellPhone { get; set; }
		public string? MedicaidNumber { get; set; }
		public string? SocialSecurityNumber { get; set; }
		public string? PreferredContact { get; set; }
		public string? AddressLine1 { get; set; }
		public string? AddressLine2 { get; set; }
		public string? City { get; set; }
		public string? State { get; set; }
		public string? Country { get; set; }
		public string? Zip { get; set; }
		public DateTime? DOB { get; set; }
		public string? Gender { get; set; }
		public int? OfficeID { get; set; }
		public string? Source { get; set; }
		public int? CoordinatorID { get; set; }
		public int? Availibility { get; set; }
		public string? SignatureImage { get; set; }
		public string? EmergencyContactFirstName { get; set; }
		public string? EmergencyContactLastName { get; set; }
		public string? EmergencyContactRelationship { get; set; }
		public string? EmergencyContactHomePhone { get; set; }
		public string? EmergencyContactCellPhone { get; set; }
		public string? PrimaryCarePhysianFirstName { get; set; }
		public string? PrimaryCarePhysianLastName { get; set; }
		public string? PrimaryCarePhysianPhone { get; set; }
		public string? PrimaryCarePhysianFax { get; set; }
		public bool? FirstAgreement { get; set; }
		public bool? SecondAgreement { get; set; }
		public bool? ThirdAgreement { get; set; }
		public bool? AuthorizationAcceptanceOne { get; set; }
		public bool? AuthorizationAcceptanceTwo { get; set; }
		public bool? AuthorizationAcceptanceThree { get; set; }
		public int? PatientStatus { get; set; }
		public int? Status { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int? UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string? CreatedByUserName { get; set; }
		public string? CoordinatorName { get; set; }
		public string? DepartmentName { get; set; }
		[NotMapped]
        public List<VMPatientTag>? lstTag { get; set; }
	}
	public class VMPatientLastChange 
	{
		public int HistoryID { get; set; }
		public int PatientID { get; set; }
		public int SequenceNo { get; set; }
		public string ColumnName { get; set; }
		public string OldValue { get; set; }
		public string NewValue { get; set; }
		public DateTime CreatedDate { get; set; }
		public int CreatedBy { get; set; }
	}

	public class VmPatientHistory
	{
		public int HistoryID { get; set; }
		public string ExecutorName { get; set; }
		public int PatientID { get; set; }
		public string PatientName { get; set; }
		public int DepartmentID { get; set; }
		public DateTime ExecutionDate { get; set; }
		public string ColumnName { get; set; }
		public string OldValue { get; set; }
		public string NewValue { get; set; }
	}
}
