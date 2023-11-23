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
    public class DepartmentStatusService : IDepartmentStatusService
    {
        private readonly CRMDbContext _crmDbContext;

        public DepartmentStatusService(CRMDbContext ctx)
        {
            this._crmDbContext = ctx;
        }

        /// <summary>
        /// Get all department status.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllDepartmentStatus(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<DepartmentStatus> lstDepartmentStatus = new List<DepartmentStatus>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstDepartmentStatus = await _crmDbContext.DepartmentStatus.OrderBy(x => x.DepartmentStatusID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstDepartmentStatus;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View,requestMessage.UserID, "GetAllDepartmentStatus");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID,JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllDepartmentStatus");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// get department status by id.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GetDepartmentStatusById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                DepartmentStatus objDepartmentStatus = new DepartmentStatus();
                int departmentStatusID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objDepartmentStatus = await _crmDbContext.DepartmentStatus.FirstOrDefaultAsync(x => x.DepartmentStatusID == departmentStatusID && x.Status == (int)Enums.Status.Active);
                responseMessage.ResponseObj = objDepartmentStatus;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View,requestMessage.UserID, "GetDepartmentStatusById");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID,JsonConvert.SerializeObject(requestMessage.RequestObj), "GetDepartmentStatusById");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Save and update  department status.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> SaveDepartmentStatus(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {

                DepartmentStatus objDepartmentStatus = JsonConvert.DeserializeObject<DepartmentStatus>(requestMessage?.RequestObj.ToString());

              

                if (objDepartmentStatus != null)
                {
                    if (CheckedValidation(objDepartmentStatus, responseMessage))
                    {
                        if (objDepartmentStatus.DepartmentStatusID > 0)
                        {
                            DepartmentStatus existingDepartmentStatus = await this._crmDbContext.DepartmentStatus.AsNoTracking().FirstOrDefaultAsync(x => x.DepartmentStatusID == objDepartmentStatus.DepartmentStatusID && x.Status == (int)Enums.Status.Active);
                            if (existingDepartmentStatus != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objDepartmentStatus.CreatedDate = existingDepartmentStatus.CreatedDate;
                                objDepartmentStatus.CreatedBy = existingDepartmentStatus.CreatedBy;
                                objDepartmentStatus.UpdatedDate = DateTime.Now;
                                objDepartmentStatus.UpdatedBy = requestMessage.UserID;
                                _crmDbContext.DepartmentStatus.Update(objDepartmentStatus);                               
                            }
                        }
                        else
                        {
                            objDepartmentStatus.Status = (int)Enums.Status.Active;
                            objDepartmentStatus.CreatedDate = DateTime.Now;
                            objDepartmentStatus.CreatedBy = requestMessage.UserID;
                            await _crmDbContext.DepartmentStatus.AddAsync(objDepartmentStatus);
                           
                        }

                        await _crmDbContext.SaveChangesAsync();

                        responseMessage.ResponseObj = objDepartmentStatus;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType,requestMessage.UserID, "SaveDepartmentStatus");
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID,JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveDepartmentStatus");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }


            return responseMessage;
        }
        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objDepartmentStatus"></param>
        /// <returns></returns>
        private bool CheckedValidation(DepartmentStatus objDepartmentStatus, ResponseMessage responseMessage)
        {
            if (string.IsNullOrEmpty(objDepartmentStatus.Title))
            {
                responseMessage.Message = MessageConstant.DepartmentStatusTitle;
                return false;
            }

            return true;
        }
#pragma warning restore CS8600

    }

   
}
