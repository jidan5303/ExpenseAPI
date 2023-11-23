using CRM.Common.DTO;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
	[Route("api/PatientAttachment")]
	[ApiController]
	public class PatientAttachmentController : ControllerBase
	{
		private readonly IPatientAttachmentService _patientAttachmentService;
		public PatientAttachmentController(IPatientAttachmentService patientAttachmentService)
		{
			_patientAttachmentService = patientAttachmentService;
		}

		[HttpPost("GetAllPatientAttachmentList")]
		public async Task<ResponseMessage> GetAllPatientAttachmentList(RequestMessage requestMessage)
		{
			return  await _patientAttachmentService.GetAllPatientAttachmentList(requestMessage);
		}

		[HttpPost("GetPatientAttachmentById")]
		public async Task<ResponseMessage> GetPatientAttachmentById(RequestMessage requestMessage)
		{
			return  await _patientAttachmentService.GetPatientAttachmentById(requestMessage);
		}

		[HttpPost("SavePatientAttachment")]
		public async Task<ResponseMessage> SavePatientAttachment(RequestMessage requestMessage)
		{
			return  await _patientAttachmentService.SavePatientAttachment(requestMessage);
		}
		
		[HttpPost("RemovePatientAttachment")]
		public async Task<ResponseMessage> RemovePatientAttachment(RequestMessage requestMessage)
		{
			return  await _patientAttachmentService.RemovePatientAttachment(requestMessage);
		}
	}
}
