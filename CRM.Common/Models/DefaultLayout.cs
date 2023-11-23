using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class DefaultLayout:BaseClass
    {
        public int DefaultLayoutID { get; set; }

        public int? TableID { get; set; } = 0;

        public string? Description { get; set; }

    }
}
