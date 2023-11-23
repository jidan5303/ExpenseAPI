using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class KnowledgeBaseHistory:BaseClass
    {
        public int KnowledgeBaseHistoryID { get; set; }

        public int? KnowledgeBaseID { get; set; } = 0;

        public string? KnowledgeDetail { get; set; }


    }
}
