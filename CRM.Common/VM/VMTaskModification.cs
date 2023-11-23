using CRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.VM
{
    public class VMTaskModification
    {
        //public int RelatedTo { get; set; }
        public int? AssigneeID { get; set; }
        public DateTime? DueDate { get; set; }
        public List<ScheduleTask> lstScheduleTask { get; set; }=new List<ScheduleTask>();
    }
}
