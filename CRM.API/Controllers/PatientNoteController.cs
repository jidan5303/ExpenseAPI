using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
	[Route("api/PatientNote")]
	[ApiController]
	public class PatientNoteController : ControllerBase
	{
		private readonly IPatientNotesService _patientNotesService;
		public PatientNoteController(IPatientNotesService patientNotesService)
		{
			_patientNotesService = patientNotesService;
		}

		[HttpPost("GetAllPatientNote")]
		public async Task<ResponseMessage> GetAllPatientNote(RequestMessage requestMessage)
		{
			return await _patientNotesService.GetAllPatientNote(requestMessage);
		}

		[HttpPost("SavePatientNote")]
		public async Task<ResponseMessage> SavePatientNote(RequestMessage requestMessage)
		{
			return await _patientNotesService.SavePatientNote(requestMessage);
		}
	}
}
