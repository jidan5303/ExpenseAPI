using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
	[Table("PatientAttachment")]
	public class PatientAttachment:BaseClass
	{
		public int PatientAttachmentID { get; set; }
		public int? PatientID { get; set; }
		public string? AttachmentName { get; set; }
		public string? AttachmentLink { get; set; }
		public string? FileExtension { get; set; }
		[NotMapped]
		public string? Base64File { get; set; }
	}
}
