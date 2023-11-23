using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class RoleActionMapping
    {
        public int RoleActionMappingID { get; set; }

        public int? RoleID { get; set; } = 0;

        public int? ActionID { get; set; } = 0;

        public int? PermissionID { get; set; } = 0;

    }
}
