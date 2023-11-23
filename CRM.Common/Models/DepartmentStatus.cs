using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class DepartmentStatus:BaseClass
    {
        public int DepartmentStatusID { get; set; }

        public string? Title { get; set; }

        public string? Color { get; set; }
    }
}
