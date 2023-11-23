using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Helper.AuditLog
{
#pragma warning disable CS8618
    public class AuditLogMain
    {
        public long AuditLogMainID { get; set; }
        public string InputObject { get; set; }

        public int UserId { get; set; }=0;
        public int ActionTypeID { get; set; } = 0;
        public string? MethodName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
#pragma warning restore CS8618
}
