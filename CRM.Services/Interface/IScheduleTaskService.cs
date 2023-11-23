using CRM.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services.Interface
{
    public interface IScheduleTaskService
    {
        Task<ResponseMessage> GetAllScheduleTask(RequestMessage requestMessage);
        Task<ResponseMessage> SaveScheduleTask(RequestMessage requestMessage);
        Task<ResponseMessage> GetScheduleTaskCreateInitData(RequestMessage requestMessage);
        Task<ResponseMessage> GetAllPatientTask(RequestMessage requestMessage);
        Task<ResponseMessage> GetAllAideTask(RequestMessage requestMessage);
        Task<ResponseMessage> GetAllScheduleTaskInitData(RequestMessage requestMessage);
        Task<ResponseMessage> SaveTaskModification(RequestMessage requestMessage);
        Task<ResponseMessage> DeleteScheduleTask(RequestMessage requestMessage);
    }
}
