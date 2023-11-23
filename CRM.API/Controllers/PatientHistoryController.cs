using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/PatientHistory")]
    [ApiController]
    public class PatientHistoryController : ControllerBase
    {
        private readonly IPatientHistoryService _patientHistoryService;
        public PatientHistoryController(IPatientHistoryService userService)
        {
            this._patientHistoryService = userService;
        }

        [HttpPost("GetAllPatientHistory")]
        public async Task<ResponseMessage> GetAllPatientHistory(RequestMessage requestMessage)
        {
            return await _patientHistoryService.GetAllPatientHistory(requestMessage);
        }

        [HttpPost("GePatientHistoryById")]
        public async Task<ResponseMessage> GetPatientHistoryById(RequestMessage requestMessage)
        {
            return await this._patientHistoryService.GetPatientHistoryById(requestMessage);
        }

        [HttpPost("SavePatientHistory")]
        public async Task<ResponseMessage> SavePatientHistory(RequestMessage requestMessage)
        {
            return await _patientHistoryService.SavePatientHistory(requestMessage);
        }
    }
}
