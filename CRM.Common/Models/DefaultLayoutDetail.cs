using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class DefaultLayoutDetail
    {
        public int DefaultLayoutDetailID { get; set; }

        public int? DefaultLayoutID { get; set; } = 0;

        public int? DefaultTableColumnID { get; set; } = 0;

    }
}
