using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
	[Route("api/CaregiverNote")]
	[ApiController]
	public class CaregiverNoteController : ControllerBase
	{
		private readonly ICaregiverNoteService _caregiverNote;
		public CaregiverNoteController(ICaregiverNoteService caregiverNote)
		{
			_caregiverNote = caregiverNote;
		}

		[HttpPost("SaveCaregiverNote")]
		public async Task<ResponseMessage> SaveCaregiverNote(RequestMessage requestMessage)
		{
			return await _caregiverNote.SaveCaregiverNote(requestMessage);
		}

		[HttpPost("GetCaregiverNoteByCaregiverID")]
		public async Task<ResponseMessage> GetCaregiverNoteByCaregiverID(RequestMessage requestMessage)
		{
			return await _caregiverNote.GetCaregiverNoteByCaregiverID(requestMessage);
		}
	}
}
