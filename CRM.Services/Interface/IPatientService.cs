using CRM.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services.Interface
{
    /// <summary>
    /// Interface
    /// </summary>
    public interface IPatientService
    {
        Task<ResponseMessage> GetAllPatient(RequestMessage requestMessage);
        Task<ResponseMessage> GetAllPatientList(RequestMessage requestMessage);
        Task<ResponseMessage> SavePatient(RequestMessage requestMessage);
        Task<ResponseMessage> GetPatientById(RequestMessage requestMessage);
        Task<ResponseMessage> GetAllPatientInitData(RequestMessage requestMessage);
        Task<ResponseMessage> GetPatientsForDropdown(RequestMessage requestMessage);
        Task<ResponseMessage> GetAllVMPatientByDepartmentID(RequestMessage requestMessage);
        Task<ResponseMessage> GetAllPatientChangeHistoryList(RequestMessage requestMessage);
        Task<ResponseMessage> RevertPatient(RequestMessage requestMessage);
        Task<ResponseMessage> GetPatientChangeHistoryList(RequestMessage requestMessage);
	}
}
