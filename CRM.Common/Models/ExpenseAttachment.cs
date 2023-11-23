using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class ExpenseAttachment : BaseClass
    {
        public long ID { get; set; }
        public long ExpenseID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
