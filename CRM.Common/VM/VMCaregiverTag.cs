using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.VM
{
    public class VMCaregiverTag
    {
        public int CaregiverAndTagMappingID { get; set; }
        public int CaregiverID { get; set; }
        public int TagID { get; set; }
        public string? TagTitle { get; set; }
        public string? Color { get; set; }
    }
}
