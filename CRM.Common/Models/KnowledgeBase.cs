using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class KnowledgeBase:BaseClass
    {
        public int KnowledgeBaseID { get; set; }

        public int? DepartmentID { get; set; } = 0;

        public string? KnowledgeDetail { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentLink { get; set; }
        public string Extention { get; set; }
        public int SequenceNo { get; set; } = 0;
        [NotMapped]
        public string? AttachmentContent { get; set; }
    }
}
