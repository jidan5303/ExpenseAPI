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
using static CRM.Common.Enums.Enums;

namespace CRM.Services
{
#pragma warning disable CS8600
    public class DepartmentTypeService : IDepartmentTypeService
    {
        private readonly CRMDbContext _crmDbContext;

        public DepartmentTypeService(CRMDbContext ctx)
        {
            this._crmDbContext = ctx;
        }

        /// <summary>
        /// Get all department type.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllDepartmentType(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<DepartmentTypes> lstDepartmentType = new List<DepartmentTypes>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstDepartmentType = await _crmDbContext.DepartmentType.OrderBy(x => x.DepartmentTypeID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstDepartmentType;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllDepartmentType");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllDepartmentType");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Get department type by Id.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GetDepartmentTypeById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                DepartmentTypes objDepartmentType = new DepartmentTypes();
                int DepartmentTypeID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objDepartmentType = await _crmDbContext.DepartmentType.FirstOrDefaultAsync(x => x.DepartmentTypeID == DepartmentTypeID);
                responseMessage.ResponseObj = objDepartmentType;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetDepartmentTypeById");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, "GetDepartmentTypeById");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Save and update department type
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> SaveDepartmentType(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {

                DepartmentTypes objDepartmentType = JsonConvert.DeserializeObject<DepartmentTypes>(requestMessage?.RequestObj.ToString());

                if (objDepartmentType != null)
                {
                    if (CheckedValidation(objDepartmentType, responseMessage))
                    {
                        if (objDepartmentType.DepartmentTypeID > 0)
                        {
                            DepartmentTypes existingDepartmentType = await this._crmDbContext.DepartmentType.AsNoTracking().FirstOrDefaultAsync(x => x.DepartmentTypeID == objDepartmentType.DepartmentTypeID);
                            if (existingDepartmentType != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                _crmDbContext.DepartmentType.Update(objDepartmentType);
                            }
                        }
                        else
                        {
                            await _crmDbContext.DepartmentType.AddAsync(objDepartmentType);

                        }

                        await _crmDbContext.SaveChangesAsync();

                        responseMessage.ResponseObj = objDepartmentType;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "SaveDepartmentType");
                    }
                    else
                    {
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Warning;
                    }

                }
                else
                {
                    //Process excetion, Development mode show real exception and production mode will show custom exception.
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = MessageConstant.SaveFailed;
                }

            }
            catch (Exception ex)
            {
                //Exception write
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveDepartmentType");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }


            return responseMessage;
        }
        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objDepartmentType"></param>
        /// <returns></returns>
        private bool CheckedValidation(DepartmentTypes objDepartmentType, ResponseMessage responseMessage)
        {
            if (string.IsNullOrEmpty(objDepartmentType.DepartmentTypeName))
            {
                responseMessage.Message = MessageConstant.DepartmentTypeName;
                return false;
            }

            return true;
        }
#pragma warning restore CS8600

    }

}
