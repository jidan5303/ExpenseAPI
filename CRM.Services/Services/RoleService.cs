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
    public class RoleService : IRoleService
    {
        private readonly CRMDbContext _crmDbContext;

        public RoleService(CRMDbContext ctx)
        {
            this._crmDbContext = ctx;
        }

        /// <summary>
        /// Get all System User
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllRole(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<Roles> lstRole = new List<Roles>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstRole = await _crmDbContext.Role.Where(x => x.Status == (int)Enums.Status.Active).OrderBy(x => x.RoleID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstRole;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllRole");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllRole");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GetRoleById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                Roles objRole = new Roles();
                int RoleID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objRole = await _crmDbContext.Role.FirstOrDefaultAsync(x => x.RoleID == RoleID && x.Status == (int)Enums.Status.Active);
                responseMessage.ResponseObj = objRole;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetRoleById");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetRoleById");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Save and update System user
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> SaveRole(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {

                Roles objRole = JsonConvert.DeserializeObject<Roles>(requestMessage?.RequestObj.ToString());



                if (objRole != null)
                {
                    if (CheckedValidation(objRole, responseMessage))
                    {
                        if (objRole.RoleID > 0)
                        {
                            Roles existingRole = await this._crmDbContext.Role.AsNoTracking().FirstOrDefaultAsync(x => x.RoleID == objRole.RoleID && x.Status == (int)Enums.Status.Active);
                            if (existingRole != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objRole.CreatedDate = existingRole.CreatedDate;
                                objRole.CreatedBy = existingRole.CreatedBy;
                                objRole.UpdatedDate = DateTime.Now;
                                objRole.UpdatedBy = requestMessage.UserID;
                                _crmDbContext.Role.Update(objRole);
                            }
                        }
                        else
                        {
                            objRole.Status = (int)Enums.Status.Active;
                            objRole.CreatedDate = DateTime.Now;
                            objRole.CreatedBy = requestMessage.UserID;
                            await _crmDbContext.Role.AddAsync(objRole);

                        }

                        await _crmDbContext.SaveChangesAsync();
                        responseMessage.ResponseObj = objRole;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "SaveRole");

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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveRole");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }

            return responseMessage;
        }

        /// <summary>
        /// Save and update System user
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> DeleteRole(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Delete;
            try
            {

                VMChangeUserRole objVMChangeUserRole = JsonConvert.DeserializeObject<VMChangeUserRole>(requestMessage?.RequestObj.ToString());

                if (objVMChangeUserRole.OldRoleID > 0)
                {
                    if (objVMChangeUserRole.NewRoleID > 0 && objVMChangeUserRole.lstSystemUser.Count > 0)
                    {
                        foreach (SystemUser objSystemUser in objVMChangeUserRole.lstSystemUser)
                        {
                            objSystemUser.Role = objVMChangeUserRole.NewRoleID;
                            _crmDbContext.SystemUser.Update(objSystemUser);
                        }
                    }
                    Roles objRole = await _crmDbContext.Role.Where(x => x.RoleID == objVMChangeUserRole.OldRoleID).FirstOrDefaultAsync();
                    if (objRole != null)
                    {
                        objRole.Status = (int)Enums.Status.Delete;
                        _crmDbContext.Role.Update(objRole);
                    }

                    await _crmDbContext.SaveChangesAsync();

                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                    responseMessage.Message = MessageConstant.DeleteSuccess;
                }
                else
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = MessageConstant.DeleteFailed;
                }

            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "DeleteRole");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }

            return responseMessage;
        }

        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objRole"></param>
        /// <returns></returns>
        private bool CheckedValidation(Roles objRole, ResponseMessage responseMessage)
        {
            if (string.IsNullOrEmpty(objRole.RoleName))
            {
                responseMessage.Message = MessageConstant.RoleName;
                return false;
            }

            return true;
        }
#pragma warning restore CS8600

    }
}
