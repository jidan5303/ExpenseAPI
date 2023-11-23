using CRM.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.VM
{
    public class VMCaregiverInit
    {
        public List<CaregiverOrganization> lstOrganization { get; set; } = new List<CaregiverOrganization>();
        public List<CaregiverAssignTo> lstAssignTo { get; set; } = new List<CaregiverAssignTo>();
        public List<Source> lstSource { get; set; } = new List<Source>();
        public List<CaregiverStatus> lstHeaderStatus { get; set; } = new List<CaregiverStatus>();
        public List<Tags> lstTags { get; set; } = new List<Tags>();
        public List<Status> lstStatus { get; set; }=new List<Status>();
    }

    public class CaregiverOrganization
    {
        public int OrganizationID { get; set; }
        public string? OrganizationName { get; set; }
        public int? OfficeID { get; set; }
        public int NewValue { get; set; } = 0;
        public int OldValue { get; set; } = 0;
    }
    public class CaregiverAssignTo
    {
        public int? AssignID { get; set; }
        public string? AssignName { get; set; }
        public int TotalAssignValue { get; set; } = 0;

    }
    public class Source
    {
        public string SourceName { get; set; }
        public int TotalSourceValue { get; set; } = 0;
    }

    public class Tags
    {
        public int? TagID { get; set; }
        public string? TagTitle { get; set; }
        public string? Color { get; set; }
        public int TotalTagsValue { get; set; } = 0;

    }

    public class CaregiverStatus
    {
        public int? StatusID { get; set; }
        public string StatusName { get; set; }
        public string ColorCode { get; set; }
        public int TotalSatusValue { get; set; } = 0;

    }

}

