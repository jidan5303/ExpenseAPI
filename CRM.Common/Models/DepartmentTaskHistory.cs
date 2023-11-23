using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class DepartmentTaskHistory:BaseClass
    {
        public int DepartmentTaskHistoryID { get; set; }

        public int? DepartmentTaskID { get; set; } = 0;

        public string? TaskDescription { get; set; }

        public int? AssignTo { get; set; } = 0;

        public int? PriorityLevel { get; set; }=0;

        public DateTime? DueDate { get; set; }
    }

}
