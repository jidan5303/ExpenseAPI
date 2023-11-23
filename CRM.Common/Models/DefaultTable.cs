using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class DefaultTable:BaseClass
    {
        public int DefaultTableID { get; set; }

        public string? TableName { get; set; }

        public bool? ShowForLayout { get; set; }=false;

    }
}
