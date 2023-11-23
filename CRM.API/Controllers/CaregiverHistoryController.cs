using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/CaregiverHistory")]
    [ApiController]
    public class CaregiverHistoryController : ControllerBase
    {
        private readonly ICaregiverHistoryService _caregiverHistoryService;
        public CaregiverHistoryController(ICaregiverHistoryService userService)
        {
            this._caregiverHistoryService = userService;
        }

        [HttpPost("GetAllCaregiverHistory")]
        public async Task<ResponseMessage> GetAllCaregiverHistory(RequestMessage requestMessage)
        {
            return await _caregiverHistoryService.GetAllCaregiverHistory(requestMessage);
        }

        [HttpPost("GeCaregiverHistoryById")]
        public async Task<ResponseMessage> GetCaregiverHistoryById(RequestMessage requestMessage)
        {
            return await this._caregiverHistoryService.GetCaregiverHistoryById(requestMessage);
        }

        [HttpPost("SaveCaregiverHistory")]
        public async Task<ResponseMessage> SaveCaregiverHistory(RequestMessage requestMessage)
        {
            return await _caregiverHistoryService.SaveCaregiverHistory(requestMessage);
        }
    }
}
