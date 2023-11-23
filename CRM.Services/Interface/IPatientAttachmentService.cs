using CRM.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services.Interface
{
	public interface IPatientAttachmentService
	{
		Task<ResponseMessage> GetAllPatientAttachmentList(RequestMessage requestMessage);
		Task<ResponseMessage> SavePatientAttachment(RequestMessage requestMessage);
		Task<ResponseMessage> GetPatientAttachmentById(RequestMessage requestMessage);
		Task<ResponseMessage> RemovePatientAttachment(RequestMessage requestMessage);
	}
}
