using CRM.Common.Constants;
using CRM.Common.DTO;
using CRM.Common.Enums;
using CRM.Common.Helper;
using CRM.Common.Models;
using CRM.Common.VM;
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
    public class DepartmentService : IDepartmentService
    {
        private readonly CRMDbContext _crmDbContext;

        public DepartmentService(CRMDbContext ctx)
        {
            this._crmDbContext = ctx;
        }

        /// <summary>
        /// Get all  departmnet.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllDepartment(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<VMDepartment> lstVMDepartment = new List<VMDepartment>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstVMDepartment = await _crmDbContext.VMDepartment.OrderBy(x => x.DepartmentID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstVMDepartment;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllDepartment");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllDepartment");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// get department by id.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GetDepartmentById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                Department objDepartment = new Department();
                int departmentID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objDepartment = await _crmDbContext.Department.AsNoTracking().FirstOrDefaultAsync(x => x.DepartmentID == departmentID && x.Status == (int)Enums.Status.Active);
                responseMessage.ResponseObj = objDepartment;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetDepartmentById");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetDepartmentById");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Save and update department
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> SaveDepartment(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {

                Department objDepartment = JsonConvert.DeserializeObject<Department>(requestMessage?.RequestObj.ToString());

                if (objDepartment != null)
                {
                    if (CheckedValidation(objDepartment, responseMessage))
                    {
                        if (objDepartment.DepartmentID > 0)
                        {
                            Department existingDepartment = await this._crmDbContext.Department.AsNoTracking().FirstOrDefaultAsync(x => x.DepartmentID == objDepartment.DepartmentID && x.Status == (int)Enums.Status.Active);
                            if (existingDepartment != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objDepartment.CreatedDate = existingDepartment.CreatedDate;
                                objDepartment.CreatedBy = existingDepartment.CreatedBy;
                                objDepartment.UpdatedDate = DateTime.Now;
                                objDepartment.UpdatedBy = requestMessage.UserID;
                                _crmDbContext.Department.Update(objDepartment);
                            }
                        }
                        else
                        {
                            objDepartment.Status = (int)Enums.Status.Active;
                            objDepartment.CreatedDate = DateTime.Now;
                            objDepartment.CreatedBy = requestMessage.UserID;
                            await _crmDbContext.Department.AddAsync(objDepartment);
                           
                        }

                        await _crmDbContext.SaveChangesAsync();

                        responseMessage.ResponseObj=objDepartment;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "SaveDepartment");
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveDepartment");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }


            return responseMessage;
        }
        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objDepartment"></param>
        /// <returns></returns>
        private bool CheckedValidation(Department objDepartment, ResponseMessage responseMessage)
        {
            if (string.IsNullOrEmpty(objDepartment.DepartmentName))
            {
                responseMessage.Message = MessageConstant.DepartmentName;
                return false;
            }
            bool isExistDepartmentName = _crmDbContext.Department.Any(x=> x.DepartmentName.ToLower() == objDepartment.DepartmentName.ToLower() && objDepartment.DepartmentID <=0);
            if (isExistDepartmentName)
            {
                responseMessage.Message = MessageConstant.DepartmentNameExist;
                return false;
            }

            return true;
        }
#pragma warning restore CS8600

    }



}
