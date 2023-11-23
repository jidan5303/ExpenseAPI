using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class CaregiverHistory:BaseClass
    {
        public int CaregiverHistoryID { get; set; }

        public int CaregiverID { get; set; } = 0;

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Gender { get; set; }

        public string? PhoneNumber { get; set; }

        public string? SecondaryPhonNumber { get; set; }

        public string? Email { get; set; }

        public string? AddressLine1 { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Zip { get; set; }

        public int? Availibility { get; set; } = 0;

        public bool? HaveCar { get; set; }=false;

        public string? PrimaryLanguage { get; set; }

        public string? SecondaryLanguage { get; set; }
        public int? EmploymentStatus { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? EmergencyContactFirstName { get; set; }
        public string? EmergencyContactLastName { get; set; }
        public int? Relationship { get; set; } = 0;
        public string? EmergencyContactHomePhone { get; set; }
        public string? EmergencyContactCellPhone { get; set; }


    }
}
