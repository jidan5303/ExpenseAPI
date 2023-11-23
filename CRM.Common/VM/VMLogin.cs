using CRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.VM
{
    public class VMLogin
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool SSOLogin { get; set; }
        public string SSOAccessToken { get; set; }
        public string Token { get; set; }
        public int SystemUserID { get; set; } = 0;
        public int RoleID { get; set; } = 0;
        public List<Permission> lstPermissions { get; set; } = new List<Permission>();
        public List<VMDepartment> lstPatientDepartment { get; set; } = new List<VMDepartment>();
        public List<VMDepartment> lstAideDepartment { get; set; } = new List<VMDepartment>();
        public List<Organization> lstUserOrganization { get; set; } = new List<Organization>();

    }
}
