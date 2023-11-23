using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class ExpensePermission
    {
        [Key]
        public int PermissionID { get; set; }
        public string RequestURL { get; set; }
        public string Role { get; set; }
    }
}
