using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
#pragma warning disable CS8618
    public class CaregiverHistoryNew
    {
        public int HistoryID { get; set; }

        public int CaregiverID { get; set; }

        public int SequenceNo { get; set; } = 0;

        public string? ColumnName { get; set; }

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; } = 0;

    }
#pragma warning restore CS8618
}
