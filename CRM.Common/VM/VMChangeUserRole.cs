using CRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.VM
{
    public class VMChangeUserRole
    {
        public int NewRoleID { get; set; }
        public int OldRoleID { get; set; }
        public List<SystemUser> lstSystemUser { get; set; } = new List<SystemUser>();
    }
}
