using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class ExpenseUserSession
    {
        [Key]
        public int SessionID { get; set; }
        public string Token { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public int Active { get; set; }
    }
}
