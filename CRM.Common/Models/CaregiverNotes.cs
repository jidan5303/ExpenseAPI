using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class CaregiverNotes:BaseClass
    {
        public int CaregiverNoteID { get; set; }
        public int? CaregiverID { get; set; } = 0;
        public string Note { get; set; }       
    }
}
