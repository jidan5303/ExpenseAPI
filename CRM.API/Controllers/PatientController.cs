using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/Patient")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        public PatientController(IPatientService userService)
        {
            this._patientService = userService;
        }

        [HttpPost("GetAllPatient")]
        public async Task<ResponseMessage> GetAllPatient(RequestMessage requestMessage)
        {
            return await _patientService.GetAllPatient(requestMessage);
        }

        [HttpPost("GetAllVMPatient")]
        public async Task<ResponseMessage> GetAllPatientList(RequestMessage requestMessage)
        {
            return await _patientService.GetAllPatientList(requestMessage);
        }

        [HttpPost("GetAllVMPatientByDepartmentID")]
        public async Task<ResponseMessage> GetAllVMPatientByDepartmentID(RequestMessage requestMessage)
        {
            return await _patientService.GetAllVMPatientByDepartmentID(requestMessage);
        }

        [HttpPost("GetAllPatientInitData")]
        public async Task<ResponseMessage> GetAllPatientInitData(RequestMessage requestMessage)
        {
            return await _patientService.GetAllPatientInitData(requestMessage);
        }

        [HttpPost("GePatientById")]
        public async Task<ResponseMessage> GetPatientById(RequestMessage requestMessage)
        {
            return await this._patientService.GetPatientById(requestMessage);
        }

        [HttpPost("SavePatient")]
        public async Task<ResponseMessage> SavePatient(RequestMessage requestMessage)
        {
            return await _patientService.SavePatient(requestMessage);
        }

        [HttpPost("GetPatientsForDropdown")]
        public async Task<ResponseMessage> GetPatientsForDropdown(RequestMessage requestMessage)
        {
            return await _patientService.GetPatientsForDropdown(requestMessage);
        }

        [HttpPost("GetAllPatientChangeHistoryList")]
        public async Task<ResponseMessage> GetAllPatientChangeHistoryList(RequestMessage requestMessage)
        {
            return await _patientService.GetAllPatientChangeHistoryList(requestMessage);
        }

        [HttpPost("RevertPatient")]
        public async Task<ResponseMessage> RevertPatient(RequestMessage requestMessage)
        {
            return await _patientService.RevertPatient(requestMessage);
        }
        
        [HttpPost("GetPatientChangeHistoryList")]
        public async Task<ResponseMessage> GetPatientChangeHistoryList(RequestMessage requestMessage)
        {
            return await _patientService.GetPatientChangeHistoryList(requestMessage);
        }
    }
}
