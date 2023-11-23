using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class SystemUserDepartmentMapping
    {
        public int SystemUserDepartmentMappingID { get; set; }
        public int SystemUserID { get; set; }
        public int DepartmentID { get; set; }
        public int DepartmentTypeID { get; set; }
    }
}
