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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CRM.Services
{
#pragma warning disable CS8600
    public class PermissionService : IPermissionService
    {
        private readonly CRMDbContext _crmDbContext;

        public PermissionService(CRMDbContext ctx)
        {
            this._crmDbContext = ctx;
        }

        /// <summary>
        /// Get all Permission
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllPermission(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<Permission> lstPermission = new List<Permission>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstPermission = await _crmDbContext.Permission.Where(x => x.Sequence != 0 && x.Status != (int)Enums.Status.Delete).OrderBy(x => x.Sequence)
                        .Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();

                responseMessage.TotalCount = lstPermission.Count;
                responseMessage.ResponseObj = lstPermission;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View,requestMessage.UserID, "GetAllPermission");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID,JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllPermission");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// get permission by id
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GetPermissionById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                Permission objPermission = new Permission();
                int PermissionID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objPermission = await _crmDbContext.Permission.FirstOrDefaultAsync(x => x.PermissionID == PermissionID && x.Status == (int)Enums.Status.Active);
                responseMessage.ResponseObj = objPermission;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View,requestMessage.UserID, "GetPermissionById");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID,JsonConvert.SerializeObject(requestMessage.RequestObj), "GetPermissionById");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// </summary>
        /// get permission by role id
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GetPermissionByRoleId(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<Permission> lstPermission = new List<Permission>();
                int roleID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                //objPermission = await _crmDbContext.Permission.FirstOrDefaultAsync(x => x.role == PermissionID && x.Status == (int)Enums.Status.Active);
                //responseMessage.ResponseObj = objPermission;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View,requestMessage.UserID, "GetPermissionById");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID,JsonConvert.SerializeObject(requestMessage.RequestObj), "GetPermissionById");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Save and update Permission
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> SavePermission(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {

                Permission objPermission = JsonConvert.DeserializeObject<Permission>(requestMessage?.RequestObj.ToString());

                if (objPermission != null)
                {
                    if (CheckedValidation(objPermission, responseMessage))
                    {
                        if (objPermission.PermissionID > 0)
                        {
                            Permission existingPermission = await this._crmDbContext.Permission.AsNoTracking().FirstOrDefaultAsync(x => x.PermissionID == objPermission.PermissionID && x.Status != (int)Enums.Status.Delete);
                            if (existingPermission != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objPermission.CreatedDate = existingPermission.CreatedDate;
                                objPermission.CreatedBy = existingPermission.CreatedBy;
                                objPermission.UpdatedDate = DateTime.Now;
                                objPermission.UpdatedBy = requestMessage.UserID;
                                _crmDbContext.Permission.Update(objPermission);
                            }
                        }
                        else
                        {
                            int maxSequence = await _crmDbContext.Permission.MaxAsync(x => x.Sequence);
                            objPermission.Sequence = maxSequence + 1;

                            objPermission.Status = (int)Enums.Status.Active;
                            objPermission.CreatedDate = DateTime.Now;
                            objPermission.CreatedBy = requestMessage.UserID;
                            await _crmDbContext.Permission.AddAsync(objPermission);
                          
                        }
                        await _crmDbContext.SaveChangesAsync();
                        responseMessage.ResponseObj = objPermission;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType,requestMessage.UserID, "SavePermission");

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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID,JsonConvert.SerializeObject(requestMessage.RequestObj), "SavePermission");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }

            return responseMessage;
        }


        /// <summary>
        /// re sequence the permissions
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> SequencePermissions(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {
                List<Permission> lstPermission = JsonConvert.DeserializeObject<List<Permission>>(requestMessage?.RequestObj.ToString());
                List<Permission> lstSequencialPermission = new List<Permission>();

                if (lstPermission.Count > 0)
                {
                    foreach (Permission objPermission in lstPermission)
                    {
                        Permission existingPermission = await _crmDbContext.Permission.AsNoTracking().
                            FirstOrDefaultAsync(x => x.PermissionID == objPermission.PermissionID && x.Status != (int)Enums.Status.Delete);

                        if (existingPermission != null)
                        {
                            actionType = (int)Enums.ActionType.Update;
                            objPermission.CreatedDate = existingPermission.CreatedDate;
                            objPermission.CreatedBy = existingPermission.CreatedBy;
                            objPermission.UpdatedDate = DateTime.Now;
                            objPermission.UpdatedBy = requestMessage.UserID;
                            _crmDbContext.Permission.Update(objPermission);
                        }
                    }
                    await _crmDbContext.SaveChangesAsync();

                    lstSequencialPermission = await _crmDbContext.Permission.Where(x => x.Sequence != 0 && x.Status != (int)Enums.Status.Delete).OrderBy(x => x.Sequence).ToListAsync();
                    
                    responseMessage.ResponseObj = lstSequencialPermission;
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                    responseMessage.Message = MessageConstant.SavedSuccessfully;
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SavePermission");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }

            return responseMessage;
        }

        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objPermission"></param>
        /// <returns></returns>
        private bool CheckedValidation(Permission objPermission, ResponseMessage responseMessage)
        {
            if (string.IsNullOrEmpty(objPermission.PermissionName))
            {
                responseMessage.Message = MessageConstant.PermissionName;
                return false;
            }

            return true;
        }
#pragma warning restore CS8600

    }

  
}
