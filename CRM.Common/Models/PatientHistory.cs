using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class PatientHistory:BaseClass
    {

        public int PatientHistoryID { get; set; } = 0;
        public int PatientID { get; set; } = 0;

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? HomePhone { get; set; }

        public string? CellPhone { get; set; }

        public string? MedicaidNumber { get; set; }

        public string? SocialSecurityNumber { get; set; }

        public string? PreferredContact { get; set; }

        public string? AddressLine1 { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Country { get; set; }

        public string? Zip { get; set; }

        public DateTime? DOB { get; set; }

        public string? Gender { get; set; }

        public int? OfficeID { get; set; } = 0;

        public string? Source { get; set; }

        public int? Coordinator { get; set; } = 0;

        public int? Availibility { get; set; } = 0;

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

        public bool? FirstAgreement { get; set; } = false;

        public bool? SecondAgreement { get; set; } = false;

        public bool? ThirdAgreement { get; set; } = false;

        public bool? AuthorizationAcceptanceOne { get; set; } = false;

        public bool? AuthorizationAcceptanceTwo { get; set; } = false;

        public bool? AuthorizationAcceptanceThree { get; set; } = false;
    }
}
