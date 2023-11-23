using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class ExpenseType : BaseClass
    {
        public int ID { get; set; }
        public int ExpenseCategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
