using CRM.Common.DTO;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
	[Route("api/ScheduleTask")]
	[ApiController]
	public class ScheduleTaskController : ControllerBase
	{
		private readonly IScheduleTaskService _scheduleTaskService;
		public ScheduleTaskController(IScheduleTaskService scheduleTaskService)
		{
			_scheduleTaskService = scheduleTaskService;
		}

		[HttpPost("GetAllScheduleTask")]
		public async Task<ResponseMessage> GetAllScheduleTask(RequestMessage requestMessage)
		{
			return  await _scheduleTaskService.GetAllScheduleTask(requestMessage);
		}
		
		[HttpPost("SaveScheduleTask")]
		public async Task<ResponseMessage> SaveScheduleTask(RequestMessage requestMessage)
		{
			return await _scheduleTaskService.SaveScheduleTask(requestMessage);
		}
		
		[HttpPost("GetScheduleTaskCreateInitData")]
		public async Task<ResponseMessage> GetScheduleTaskCreateInitData(RequestMessage requestMessage)
		{
			return  await _scheduleTaskService.GetScheduleTaskCreateInitData(requestMessage);
		}
		
		[HttpPost("GetAllPatientTask")]
		public async Task<ResponseMessage> GetAllPatientTask(RequestMessage requestMessage)
		{
			return  await _scheduleTaskService.GetAllPatientTask(requestMessage);
		}
		
		[HttpPost("GetAllAideTask")]
		public async Task<ResponseMessage> GetAllAideTask(RequestMessage requestMessage)
		{
			return  await _scheduleTaskService.GetAllAideTask(requestMessage);
		}
		
		[HttpPost("GetAllScheduleTaskInitData")]
		public async Task<ResponseMessage> GetAllScheduleTaskInitData(RequestMessage requestMessage)
		{
			return  await _scheduleTaskService.GetAllScheduleTaskInitData(requestMessage);
		}


		[HttpPost("SaveTaskModification")]
		public async Task<ResponseMessage> SaveTaskModification(RequestMessage requestMessage)
		{
			return await _scheduleTaskService.SaveTaskModification(requestMessage);
		}

		[HttpPost("DeleteScheduleTask")]
		public async Task<ResponseMessage> DeleteScheduleTask(RequestMessage requestMessage)
		{
			return await _scheduleTaskService.DeleteScheduleTask(requestMessage);
		}
	}
}
