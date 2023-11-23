using CRM.Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.VM
{

    public class VMCaregivers
    {
        public List<VMCaregiver> lstCaregiver { get; set; } = new List<VMCaregiver>();
        public List<VMCaregiverLastChange> lstCaregiverHistory { get; set; } = new List<VMCaregiverLastChange>();
    }



    public class VMCaregiver
    {
        public int CaregiverID { get; set; } = 0;
        public int? DepartmentID { get; set; } = 0;
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int Gender { get; set; } = 0;

        public string? PhoneNumber { get; set; }

        public string? SecondaryPhonNumber { get; set; }

        public string? Email { get; set; }

        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Zip { get; set; }

        public int? Availibility { get; set; } = 0;

        public bool? HaveCar { get; set; } = false;

        public string? PrimaryLanguage { get; set; }

        public string? SecondaryLanguage { get; set; }

        public int CreatedBy { get; set; } = 0;

        public DateTime? CreatedDate { get; set; }

        public int? UpdatedBy { get; set; } = 0;

        public DateTime? UpdatedDate { get; set; } = CommonConstant.DeafultDate;

        public int? Status { get; set; } = 1;
        public int? EmploymentStatus { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? EmergencyContactFirstName { get; set; }
        public string? EmergencyContactLastName { get; set; }
        public int? Relationship { get; set; } = 0;
        public string? EmergencyContactHomePhone { get; set; }
        public string? EmergencyContactCellPhone { get; set; }
        public string FullName { get; set; }
        public string CreatedByUserName { get; set; }
        public int CoordinatorID { get; set; }
        public string? CoordinatorName { get; set; }
        public string? DepartmentName { get; set; }
        public int OfficeID { get; set; } = 0;

        [NotMapped]
        public string County { get; set; }
        [NotMapped]
        public string? EmploymentStatusName { get; set; }
        [NotMapped]
        public string? Color { get; set; }
        [NotMapped]
        public string LeadSource { get; set; }
        [NotMapped]
        public string OnboardingCoordinator { get; set; }
        [NotMapped]
        public string CoordinationOnboardingCoordinator { get; set; }
        [NotMapped]
        public string Assignee { get; set; }
        [NotMapped]
        public string AssignedTo { get; set; }
        [NotMapped]
        public string InPlan { get; set; }
        [NotMapped]
        public decimal RateOfPay { get; set; } = 0;
        [NotMapped]
        public List<Tags> lstTag { get; set; } = new List<Tags>();
        [NotMapped]
        public decimal Progress { get; set; } = 0;

    }




    public class VMCaregiverLastChange
    {
        public int HistoryID { get; set; }

        public int CaregiverID { get; set; }

        public int SequenceNo { get; set; }

        public string? ColumnName { get; set; }

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int CreatedBy { get; set; }


    }
}
