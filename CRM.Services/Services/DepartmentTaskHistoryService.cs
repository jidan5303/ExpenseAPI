using CRM.Common.Constants;
using CRM.Common.DTO;
using CRM.Common.Enums;
using CRM.Common.Helper;
using CRM.Common.Models;
using CRM.DataAccess;
using CRM.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services
{
#pragma warning disable CS8600
    public class DepartmentTaskHistoryService : IDepartmentTaskHistoryService
    {
        private readonly CRMDbContext _crmDbContext;

        public DepartmentTaskHistoryService(CRMDbContext ctx)
        {
            this._crmDbContext = ctx;
        }

        /// <summary>
        /// Get all Department Task History
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllDepartmentTaskHistory(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<DepartmentTaskHistory> lstDepartmentTaskHistory = new List<DepartmentTaskHistory>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstDepartmentTaskHistory = await _crmDbContext.DepartmentTaskHistory.OrderBy(x => x.DepartmentTaskHistoryID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstDepartmentTaskHistory;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View,requestMessage.UserID, "GetAllDepartmentTaskHistory");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID,JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllDepartmentTaskHistory");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Get Department Task History By Id
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GetDepartmentTaskHistoryById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                DepartmentTaskHistory objDepartmentTaskHistory = new DepartmentTaskHistory();
                int DepartmentTaskHistoryID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objDepartmentTaskHistory = await _crmDbContext.DepartmentTaskHistory.FirstOrDefaultAsync(x => x.DepartmentTaskHistoryID == DepartmentTaskHistoryID && x.Status == (int)Enums.Status.Active);
                responseMessage.ResponseObj = objDepartmentTaskHistory;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View,requestMessage.UserID, "GetDepartmentTaskHistoryById");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID,JsonConvert.SerializeObject(requestMessage.RequestObj), "GetDepartmentTaskHistoryById");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Save and update department task history.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> SaveDepartmentTaskHistory(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {

                DepartmentTaskHistory objDepartmentTaskHistory = JsonConvert.DeserializeObject<DepartmentTaskHistory>(requestMessage?.RequestObj.ToString());

                

                if (objDepartmentTaskHistory != null)
                {
                    if (CheckedValidation(objDepartmentTaskHistory, responseMessage))
                    {
                        if (objDepartmentTaskHistory.DepartmentTaskHistoryID > 0)
                        {
                            DepartmentTaskHistory existingDepartmentTaskHistory = await this._crmDbContext.DepartmentTaskHistory.AsNoTracking().FirstOrDefaultAsync(x => x.DepartmentTaskHistoryID == objDepartmentTaskHistory.DepartmentTaskHistoryID && x.Status == (int)Enums.Status.Active);
                            if (existingDepartmentTaskHistory != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objDepartmentTaskHistory.CreatedDate = existingDepartmentTaskHistory.CreatedDate;
                                objDepartmentTaskHistory.CreatedBy = existingDepartmentTaskHistory.CreatedBy;
                                objDepartmentTaskHistory.UpdatedDate = DateTime.Now;
                                objDepartmentTaskHistory.UpdatedBy = requestMessage.UserID;
                                _crmDbContext.DepartmentTaskHistory.Update(objDepartmentTaskHistory);                               
                            }
                        }
                        else
                        {
                            objDepartmentTaskHistory.Status = (int)Enums.Status.Active;
                            objDepartmentTaskHistory.CreatedDate = DateTime.Now;
                            objDepartmentTaskHistory.CreatedBy = requestMessage.UserID;
                           await _crmDbContext.DepartmentTaskHistory.AddAsync(objDepartmentTaskHistory);
                 
                        }

                        await _crmDbContext.SaveChangesAsync();

                        responseMessage.ResponseObj = objDepartmentTaskHistory;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType,requestMessage.UserID, "SaveDepartmentTaskHistory");

                    }
                    else
                    {
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Warning;
                    }

                    
                }
                else
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = MessageConstant.SaveFailed;
                }

            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID,JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveDepartmentTaskHistory");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }


            return responseMessage;
        }
        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objDepartmentTaskHistory"></param>
        /// <returns></returns>
        private bool CheckedValidation(DepartmentTaskHistory objDepartmentTaskHistory, ResponseMessage responseMessage)
        {
            if (string.IsNullOrEmpty(objDepartmentTaskHistory.TaskDescription))
            {
                responseMessage.Message = MessageConstant.TaskDescription;
                return false;
            }
            if (objDepartmentTaskHistory.DepartmentTaskID <= 0)
            {
                responseMessage.Message = MessageConstant.DepartmentTaskID;
                return false;
            }

            return true;
        }
#pragma warning restore CS8600

    }


}
