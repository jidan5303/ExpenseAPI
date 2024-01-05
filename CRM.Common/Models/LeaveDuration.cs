using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class LeaveDuration
    {
        public int LeaveDurationID { get; set; }
        public int LeaveTypeID { get; set; }
        public int Duration { get; set; }
    }
}
