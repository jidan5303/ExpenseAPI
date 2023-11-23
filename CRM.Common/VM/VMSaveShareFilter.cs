using CRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.VM
{
    public class VMSaveShareFilter
    {
        public CustomFilter CustomFilter = new CustomFilter();
        public List<VMUserAndShareFilterDepartment> lstShareFilterDepartment = new List<VMUserAndShareFilterDepartment>();
    }
    public class VMShareAllFilterDepaertment
    {
        public CustomFilter CustomFilter = new CustomFilter();
        public List<Department> lstDepartment = new List<Department>();
    }
}
