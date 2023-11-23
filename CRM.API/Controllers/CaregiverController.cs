using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/Caregiver")]
    [ApiController]
    public class CaregiverController : ControllerBase
    {
        private readonly ICaregiverService _caregiverService;
        public CaregiverController(ICaregiverService userService)
        {
            this._caregiverService = userService;
        }

        [HttpPost("GetAllCaregiverInitData")]
        public async Task<ResponseMessage> GetAllCaregiverInitData(RequestMessage requestMessage)
        {
            return await _caregiverService.GetAllCaregiverInitData(requestMessage);
        }

        [HttpPost("GetAllCaregiver")]
        public async Task<ResponseMessage> GetAllCaregiver(RequestMessage requestMessage)
        {
            return await _caregiverService.GetAllCaregiver(requestMessage);
        }

        [HttpPost("GetAllCaregiverList")]
        public async Task<ResponseMessage> GetAllCaregiverList(RequestMessage requestMessage)
        {
            return await _caregiverService.GetAllCaregiverList(requestMessage);
        }

        [HttpPost("GetAllCaregiverListByDepartmentID")]
        public async Task<ResponseMessage> GetAllCaregiverListByDepartmentID(RequestMessage requestMessage)
        {
            return await _caregiverService.GetAllCaregiverListByDepartmentID(requestMessage);

        }

        [HttpPost("GeCaregiverById")]
        public async Task<ResponseMessage> GetCaregiverById(RequestMessage requestMessage)
        {
            return await this._caregiverService.GetCaregiverById(requestMessage);
        }

        [HttpPost("SaveCaregiver")]
        public async Task<ResponseMessage> SaveCaregiver(RequestMessage requestMessage)
        {
            return await _caregiverService.SaveCaregiver(requestMessage);
        }

        [HttpPost("GetCaregiverForDropdown")]
        public async Task<ResponseMessage> GetCaregiverForDropdown(RequestMessage requestMessage)
        {
            return await _caregiverService.GetCaregiverForDropdown(requestMessage);
        }

        [HttpPost("GetAllCaregiverWithAllSearch")]
        public async Task<ResponseMessage> GetAllCaregiverWithAllSearch(RequestMessage requestMessage)
        {
            return await _caregiverService.GetAllCaregiverWithAllSearch(requestMessage);

        }

        [HttpPost("GetAllCaregiverGiverHistoryByDepartmentID")]
        public async Task<ResponseMessage> GetAllCaregiverGiverHistoryByDepartmentID(RequestMessage requestMessage)
        {
            return await _caregiverService.GetAllCaregiverGiverHistoryByDepartmentID(requestMessage);

        }

        [HttpPost("RevertCaregiver")]
        public async Task<ResponseMessage> RevertCaregiver(RequestMessage requestMessage)
        {
            return await _caregiverService.RevertCaregiver(requestMessage);
        }

        [HttpPost("GetAllStatus")]
        public async Task<ResponseMessage> GetAllStatus(RequestMessage requestMessage)
        {
            return await _caregiverService.GetAllStatus(requestMessage);
        }
        [HttpPost("GetAllCaregiverGiverHistoryByCaregiverID")]
        public async Task<ResponseMessage> GetAllCaregiverGiverHistoryByCaregiverID(RequestMessage requestMessage)
        {
            return await _caregiverService.GetAllCaregiverGiverHistoryByCaregiverID(requestMessage);

        }
    }
}
