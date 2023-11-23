using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.VM
{
    public class VMCaregiverGiverHistory
    {
        public int HistoryID { get; set; }
        public int? DepartmentID { get; set; }

        public string? ExecutorName { get; set; }

        public int CaregiverID { get; set; }

        public string? CaregiverName { get; set; }

        public DateTime ExecutionDate { get; set; }

        public string? ColumnName { get; set; }

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }
    }
}
