using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class Actions:BaseClass
    {
        public int ActionID { get; set; }

        public int? PermissionID { get; set; } = 0;

        public string? ActionName { get; set; }
     
    }
}
