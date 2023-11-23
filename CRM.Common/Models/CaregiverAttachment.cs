using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class CaregiverAttachment:BaseClass
    {
        public int CaregiverAttachmentID { get; set; }
        public int CaregiverID { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentLink { get; set; }
        public string Extention { get; set; }
        [NotMapped]
        public string? AttachmentContent { get; set; }

    }
}
