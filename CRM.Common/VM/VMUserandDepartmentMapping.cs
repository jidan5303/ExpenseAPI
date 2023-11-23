using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.VM
{
    public class VMUserAndDepartmentMapping
    {
        public int SystemUserID { get; set; } = 0;
        public int DepartmentID { get; set; }
        public string? DepartmentName { get; set; }
        public string? FullName { get; set; }

    }
    public class ShareFilterDepartment
    {
        public int DepartmentID { get; set; }
        public string? DepartmentName { get; set; }
    }

    public class VMUserAndShareFilterDepartment
    {
        public int SystemUserID { get; set; } = 0;      
        public string? FullName { get; set; }
        public List<ShareFilterDepartment> lstShareFilterDepartment { get; set; } = new List<ShareFilterDepartment>();
        
    }

}
