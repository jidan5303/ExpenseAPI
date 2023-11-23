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
    public class CustomFilterService : ICustomFilterService
    {
        private readonly CRMDbContext _crmDbContext;

        public CustomFilterService(CRMDbContext ctx)
        {
            this._crmDbContext = ctx;
        }

        /// <summary>
        /// Get Get All Custom Filter
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllCustomFilter(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<CustomFilter> lstCustomFilter = new List<CustomFilter>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstCustomFilter = await _crmDbContext.CustomFilter.OrderBy(x => x.CustomFilterID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstCustomFilter;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllCustomFilter");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllCustomFilter");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Get Get All Custom Filter
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllCustomFilterByUserNameAndPageName(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<CustomFilter> lstCustomFilter = new List<CustomFilter>();

                string pageName = string.Empty;
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;
                pageName = requestMessage.RequestObj.ToString();

                lstCustomFilter = await _crmDbContext.CustomFilter.Where(cf => cf.UserID == requestMessage.UserID && cf.PageName == pageName).OrderBy(x => x.CustomFilterID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstCustomFilter;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                responseMessage.TotalCount = (lstCustomFilter.Count > 0) ? _crmDbContext.CustomFilter.Count() : 0;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllCustomFilterByUserNameAndPAgeNAme");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllCustomFilter");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>    
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GetCustomFilterById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                CustomFilter objCustomFilter = new CustomFilter();
                int CustomFilterID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objCustomFilter = await _crmDbContext.CustomFilter.FirstOrDefaultAsync(x => x.CustomFilterID == CustomFilterID);
                responseMessage.ResponseObj = objCustomFilter;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetCustomFilterById");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetCustomFilterById");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Save and  Save Custom Filter
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> SaveCustomFilter(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {
                CustomFilter objCustomFilter = JsonConvert.DeserializeObject<CustomFilter>(requestMessage?.RequestObj.ToString());

                if (objCustomFilter != null)
                {
                    if (CheckedValidation(objCustomFilter, responseMessage))
                    {
                        if (objCustomFilter.CustomFilterID > 0)
                        {
                            CustomFilter existingCustomFilter = await _crmDbContext.CustomFilter.AsNoTracking().FirstOrDefaultAsync(x => x.CustomFilterID == objCustomFilter.CustomFilterID && x.Status == (int)Enums.Status.Active);
                            if (existingCustomFilter != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objCustomFilter.CreatedBy = (int)existingCustomFilter.UserID;
                                objCustomFilter.CreatedDate = existingCustomFilter.CreatedDate;
                                objCustomFilter.UserID = existingCustomFilter.UserID;
                                objCustomFilter.UpdatedBy = requestMessage.UserID;
                                objCustomFilter.UpdatedDate = DateTime.Now;
                                _crmDbContext.CustomFilter.Update(objCustomFilter);
                            }
                        }
                        else
                        {
                            objCustomFilter.CreatedBy = requestMessage.UserID;
                            objCustomFilter.CreatedDate = DateTime.Now;
                            objCustomFilter.UserID = requestMessage.UserID;
                            await _crmDbContext.CustomFilter.AddAsync(objCustomFilter);
                        }

                        await _crmDbContext.SaveChangesAsync();
                        responseMessage.ResponseObj = objCustomFilter;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "SaveCustomFilter");
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveCustomFilter");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }
            return responseMessage;
        }

        public async Task<ResponseMessage> DeleteCustomFilter(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {
                CustomFilter objCustomFilter = JsonConvert.DeserializeObject<CustomFilter>(requestMessage?.RequestObj.ToString());

                if (objCustomFilter != null)
                {
                    _crmDbContext.CustomFilter.Remove(objCustomFilter);
                    await _crmDbContext.SaveChangesAsync();
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                    responseMessage.Message = MessageConstant.DeleteSuccess;
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "DeleteCustomFilter");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }
        public async Task<ResponseMessage> SetDefaultCustomFilter(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {
                CustomFilter objCustomFilter = JsonConvert.DeserializeObject<CustomFilter>(requestMessage?.RequestObj.ToString());

                if (objCustomFilter != null)
                {
                    List<CustomFilter> lstCustomFilter = await _crmDbContext.CustomFilter.AsNoTracking()
                        .Where(x => x.IsDefault.Value && x.PageName == objCustomFilter.PageName && x.CustomFilterID != objCustomFilter.CustomFilterID).ToListAsync();
                    lstCustomFilter.ForEach(customFilter =>
                    {
                        if (customFilter.CustomFilterID != objCustomFilter.CustomFilterID)
                        {
                            customFilter.IsDefault = false;
                            _crmDbContext.CustomFilter.Update(customFilter);
                        }

                    });

                    _crmDbContext.CustomFilter.Update(objCustomFilter);
                    await _crmDbContext.SaveChangesAsync();
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                    responseMessage.Message = MessageConstant.SetAsDefaultSuccessfully;
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SetDefaultCustomFilter");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> GetDefaultCustomFilter(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                string pageName = requestMessage?.RequestObj.ToString();

                CustomFilter objCustomFilter = await _crmDbContext.CustomFilter
                    .Where(x => (bool)x.IsDefault && x.UserID == requestMessage.UserID && x.PageName == pageName)
                    .FirstOrDefaultAsync();
                responseMessage.ResponseObj = objCustomFilter;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetDefaultCustomFilter");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }

        /// <summary>
        /// method for save shar filter
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> SaveShareCustomFilter(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            int actionType = (int)Enums.ActionType.Insert;
            try
            {
                VMSaveShareFilter objVMSaveShareFilter = JsonConvert.DeserializeObject<VMSaveShareFilter>(requestMessage?.RequestObj.ToString());

                if (objVMSaveShareFilter != null)
                {
                    if (objVMSaveShareFilter.lstShareFilterDepartment.Count > 0)
                    {
                        foreach (VMUserAndShareFilterDepartment objShareFilter in objVMSaveShareFilter.lstShareFilterDepartment)
                        {
                            CustomFilter objCustomFilter = new CustomFilter();

                            objCustomFilter.UserID = objShareFilter.SystemUserID;
                            objCustomFilter.CustomFilterName = objVMSaveShareFilter.CustomFilter?.CustomFilterName;
                            objCustomFilter.PageName = objVMSaveShareFilter.CustomFilter?.PageName;
                            objCustomFilter.FilterObject = objVMSaveShareFilter.CustomFilter?.FilterObject;
                            objCustomFilter.CreatedBy = requestMessage.UserID;
                            objCustomFilter.CreatedDate = DateTime.Now;
                            objCustomFilter.Status = (int)Enums.Status.Active;
                            objCustomFilter.IsDefault = false;
                            await _crmDbContext.CustomFilter.AddAsync(objCustomFilter);
                        }

                        await _crmDbContext.SaveChangesAsync();
                        responseMessage.ResponseObj = objVMSaveShareFilter;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "SaveCustomFilter");
                    }
                    else
                    {
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Warning;
                        responseMessage.Message = MessageConstant.SelectUser;
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveCustomFilter");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }
            return responseMessage;
        }


        /// <summary>
        /// for share all deperatement.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> SaveShareAllDepCustomFilter(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            int actionType = (int)Enums.ActionType.Insert;
            try
            {
                VMShareAllFilterDepaertment objVMSaveShareFilter = JsonConvert.DeserializeObject<VMShareAllFilterDepaertment>(requestMessage?.RequestObj.ToString());

                if (objVMSaveShareFilter != null && objVMSaveShareFilter.CustomFilter != null)
                {
                    if (objVMSaveShareFilter.lstDepartment.Count > 0)
                    {
                        CustomFilter objexistingCustomFilter =await _crmDbContext.CustomFilter.AsNoTracking().FirstOrDefaultAsync(x=>x.UserID==objVMSaveShareFilter.CustomFilter.UserID && x.CustomFilterID== objVMSaveShareFilter.CustomFilter.CustomFilterID);


                        List<int> lstDepartmentID = objVMSaveShareFilter.lstDepartment.Select(x => x.DepartmentID).Distinct().ToList();

                        var userandDepartmentMapping = _crmDbContext.VMUserAndDepartmentMapping.Where(x => lstDepartmentID.Contains(x.DepartmentID)).AsQueryable();

                        List<int> lstSystemUserId = userandDepartmentMapping.Select(x => x.SystemUserID).Distinct().ToList();

                        if (lstSystemUserId.Count() > 0)
                        {
                            foreach (int systemid in lstSystemUserId)
                            {
                                if (objexistingCustomFilter!=null && systemid== objexistingCustomFilter.UserID)
                                {

                                }
                                else
                                {
                                    CustomFilter objCustomFilter = new CustomFilter();

                                    objCustomFilter.UserID = systemid;
                                    objCustomFilter.CustomFilterName = objVMSaveShareFilter.CustomFilter?.CustomFilterName;
                                    objCustomFilter.PageName = objVMSaveShareFilter.CustomFilter?.PageName;
                                    objCustomFilter.FilterObject = objVMSaveShareFilter.CustomFilter?.FilterObject;
                                    objCustomFilter.CreatedBy = requestMessage.UserID;
                                    objCustomFilter.CreatedDate = DateTime.Now;
                                    objCustomFilter.Status = (int)Enums.Status.Active;
                                    objCustomFilter.IsDefault = false;
                                    await _crmDbContext.CustomFilter.AddAsync(objCustomFilter);

                                }
                               
                            }

                            await _crmDbContext.SaveChangesAsync();
                            responseMessage.ResponseObj = objVMSaveShareFilter;
                            responseMessage.Message = MessageConstant.SavedSuccessfully;
                            responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                            //Log write
                            LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "SaveShareAllDepCustomFilter");

                        }
                        else
                        {
                            responseMessage.ResponseCode = (int)Enums.ResponseCode.Warning;
                            responseMessage.Message = MessageConstant.AnyuserNotMapping;
                        }
                    }
                    else
                    {
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Warning;
                        responseMessage.Message = MessageConstant.SelectUser;
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveShareAllDepCustomFilter");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }
            return responseMessage;
        }









        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objCustomFilter"></param>
        /// <returns></returns>
        private bool CheckedValidation(CustomFilter objCustomFilter, ResponseMessage responseMessage)
        {
            if (string.IsNullOrEmpty(objCustomFilter.FilterObject))
            {
                responseMessage.Message = MessageConstant.CustomFilterObject;
                return false;
            }

            return true;
        }
#pragma warning restore CS8600

    }


}
