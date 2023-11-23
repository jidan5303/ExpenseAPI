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
    public class DepartmentTaskService : IDepartmentTaskService
    {
        private readonly CRMDbContext _crmDbContext;

        public DepartmentTaskService(CRMDbContext ctx)
        {
            this._crmDbContext = ctx;
        }

        /// <summary>
        /// Get all department task.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllDepartmentTask(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<DepartmentTask> lstDepartmentTask = new List<DepartmentTask>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstDepartmentTask = await _crmDbContext.DepartmentTask.OrderBy(x => x.DepartmentTaskID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstDepartmentTask;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllDepartmentTask");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllDepartmentTask");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// get department task by id.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GetDepartmentTaskById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                DepartmentTask objDepartmentTask = new DepartmentTask();
                int departmentTaskID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objDepartmentTask = await _crmDbContext.DepartmentTask.FirstOrDefaultAsync(x => x.DepartmentTaskID == departmentTaskID && x.Status == (int)Enums.Status.Active);
                responseMessage.ResponseObj = objDepartmentTask;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetDepartmentTaskById");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetDepartmentTaskById");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }


        /// <summary>
        /// Save and update department task.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> SaveDepartmentTask(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {

                DepartmentTask objDepartmentTask = JsonConvert.DeserializeObject<DepartmentTask>(requestMessage?.RequestObj.ToString());



                if (objDepartmentTask != null)
                {
                    if (CheckedValidation(objDepartmentTask, responseMessage))
                    {
                        if (objDepartmentTask.DepartmentTaskID > 0)
                        {
                            DepartmentTask existingDepartmentTask = await this._crmDbContext.DepartmentTask.AsNoTracking().FirstOrDefaultAsync(x => x.DepartmentTaskID == objDepartmentTask.DepartmentTaskID && x.Status == (int)Enums.Status.Active);
                            if (existingDepartmentTask != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objDepartmentTask.CreatedDate = existingDepartmentTask.CreatedDate;
                                objDepartmentTask.CreatedBy = existingDepartmentTask.CreatedBy;
                                objDepartmentTask.UpdatedDate = DateTime.Now;
                                objDepartmentTask.UpdatedBy = requestMessage.UserID;
                                _crmDbContext.DepartmentTask.Update(objDepartmentTask);
                            }
                        }
                        else
                        {
                            objDepartmentTask.Status = (int)Enums.Status.Active;
                            objDepartmentTask.CreatedDate = DateTime.Now;
                            objDepartmentTask.CreatedBy = requestMessage.UserID;
                            await _crmDbContext.DepartmentTask.AddAsync(objDepartmentTask);
                         
                        }

                        await _crmDbContext.SaveChangesAsync();

                        responseMessage.ResponseObj = objDepartmentTask;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "SaveDepartmentTask");
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveDepartmentTask");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }

            return responseMessage;
        }
        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objDepartmentTask"></param>
        /// <returns></returns>
        private bool CheckedValidation(DepartmentTask objDepartmentTask, ResponseMessage responseMessage)
        {
            if (string.IsNullOrEmpty(objDepartmentTask.TaskDescription))
            {
                responseMessage.Message = MessageConstant.DepartmentTaskDescription;
                return false;
            }
            if (objDepartmentTask.DepartmentID <= 0)
            {
                responseMessage.Message = MessageConstant.DepartmentName;
                return false;
            }

            return true;
        }
#pragma warning restore CS8600

    }


}
