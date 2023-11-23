using CRM.Common.VM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class Patient:BaseClass
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
		[NotMapped]
		public virtual List<VMPatientTag>? lstTag { get; set; }
	}
}
