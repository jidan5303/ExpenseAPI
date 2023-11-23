using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class CaregiverAndTagMapping
    {
        public int CaregiverAndTagMappingID { get; set; }
        public int CaregiverID { get; set; }
        public int TagID { get; set; }
        public int Status { get; set; }
    }
}
