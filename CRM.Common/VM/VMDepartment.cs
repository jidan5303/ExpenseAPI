using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.VM
{
    public class VMDepartment
    {
        public int DepartmentID { get; set; }

        public string? DepartmentName { get; set; }

        public string? Description { get; set; }

        public int DepartmentTypeID { get; set; } = 0;

        public int? DepartmentLevel { get; set; } = 0;

        public string? PreferredEmployee { get; set; }

        public string? NotifyAfter { get; set; }

        public int? PriorityLevel { get; set; } = 0;

        public bool? MoveToNextDepartmentWhenDone { get; set; } = false;
        public string? DepartmentTypeName { get; set; }

    }
}
