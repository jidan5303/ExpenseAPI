using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class DepartmentTask:BaseClass
    {
        public int DepartmentTaskID { get; set; }

        public int? DepartmentID { get; set; } = 0;

        public string? TaskDescription { get; set; }

        public int? AssignTo { get; set; } = 0;

        public int? PriorityLevel { get; set; }=0;

        public DateTime? DueDate { get; set; }

    }
}
