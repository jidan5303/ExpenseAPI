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
using System.Text.Json;
using System.Threading.Tasks;

namespace CRM.Services
{
#pragma warning disable CS8600
    public class SystemUserService : ISystemUserService
    {
        private readonly CRMDbContext _crmDbContext;

        public SystemUserService(CRMDbContext ctx)
        {
            this._crmDbContext = ctx;
        }

        /// <summary>
        /// Get all System User
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllSystemUser(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<SystemUser> lstSystemUser = new List<SystemUser>();
                string searchtext = string.Empty;
                searchtext = requestMessage.RequestObj.ToString();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                IQueryable<SystemUser> lstUsers= (!string.IsNullOrEmpty(searchtext))? _crmDbContext.SystemUser.Where(x=>x.FullName.ToLower().Contains(searchtext.ToLower())||x.UserName.ToLower().Contains(searchtext.ToLower()) ||
                x.Email.ToLower().Contains(searchtext.ToLower()) || x.PhoneNumber.ToLower().Contains(searchtext.ToLower())) : _crmDbContext.SystemUser;
              
                lstSystemUser = await lstUsers.Where(x => x.Status == (int)Enums.Status.Active).OrderBy(x => x.SystemUserID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.TotalCount = lstSystemUser.Count();

                foreach (SystemUser user in lstSystemUser)
                {
                    user.Password = string.Empty;

                    // get all department of each user
                    List<SystemUserDepartmentMapping> lstSystemUserDepartmentMappings = await _crmDbContext.SystemUserDepartmentMapping.AsNoTracking().Where(x => x.SystemUserID == user.SystemUserID).ToListAsync();

                    if (lstSystemUserDepartmentMappings.Count > 0)
                    {
                        foreach (SystemUserDepartmentMapping objSystemUserDepartmentMapping in lstSystemUserDepartmentMappings)
                        {
                            Department objDepartment = await _crmDbContext.Department.AsNoTracking().Where(x => x.DepartmentID == objSystemUserDepartmentMapping.DepartmentID).FirstOrDefaultAsync();

                            if (objDepartment != null)
                            {
                                user.lstDepartment.Add(objDepartment);
                            }

                        }
                    }

                    // get all organization of each user
                    List<SystemUserOrganizationMapping> lstSystemUserOrganizationMappings = await _crmDbContext.SystemUserOrganizationMapping.AsNoTracking().Where(x => x.SystemUserID == user.SystemUserID).ToListAsync();

                    if (lstSystemUserOrganizationMappings.Count > 0)
                    {
                        foreach (SystemUserOrganizationMapping objSystemUserOrganizationMapping in lstSystemUserOrganizationMappings)
                        {
                            Organization objOrganization = await _crmDbContext.Organization.AsNoTracking().Where(x => x.OrganizationID == objSystemUserOrganizationMapping.OrganizationID).FirstOrDefaultAsync();

                            if (objOrganization != null)
                            {
                                user.lstOrganization.Add(objOrganization);
                            }

                        }
                    }

                }
                responseMessage.ResponseObj = lstSystemUser;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllSystemUser");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllSystemUser");
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
        public async Task<ResponseMessage> GetSystemUserById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                SystemUser objSystemUser = new SystemUser();
                int systemUserID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objSystemUser = await _crmDbContext.SystemUser.FirstOrDefaultAsync(x => x.SystemUserID == systemUserID && x.Status == (int)Enums.Status.Active);
                responseMessage.ResponseObj = objSystemUser;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetSystemUserById");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetSystemUserById");
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
        public async Task<ResponseMessage> GetSystemUserByUserName(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                SystemUser objSystemUser = new SystemUser();

                string systemUserName = requestMessage?.RequestObj?.ToString();

                objSystemUser = await _crmDbContext.SystemUser.FirstOrDefaultAsync(x => x.UserName.ToLower() == systemUserName.ToLower() && x.Status == (int)Enums.Status.Active);
                responseMessage.ResponseObj = objSystemUser;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetSystemUserByUserName");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetSystemUserById");
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
        public async Task<ResponseMessage> GetSystemUserByEmail(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                SystemUser objSystemUser = new SystemUser();
                string email = requestMessage?.RequestObj.ToString();

                objSystemUser = await _crmDbContext.SystemUser.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower() && x.Status == (int)Enums.Status.Active);
                responseMessage.ResponseObj = objSystemUser;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetSystemUserByEmail");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetSystemUserById");
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
        public async Task<ResponseMessage> SaveSystemUser(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            int actionType = (int)Enums.ActionType.Insert;
            try
            {

                SystemUser objSystemUser = JsonConvert.DeserializeObject<SystemUser>(requestMessage?.RequestObj.ToString());
                if (objSystemUser != null)
                {
                    if (CheckedValidation(objSystemUser, responseMessage))
                    {
                        if (objSystemUser.SystemUserID > 0)
                        {
                            SystemUser existingSystemUser = await this._crmDbContext.SystemUser.AsNoTracking().FirstOrDefaultAsync(x => x.SystemUserID == objSystemUser.SystemUserID && x.Status == (int)Enums.Status.Active);
                            if (existingSystemUser != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objSystemUser.Password = (string.IsNullOrEmpty(objSystemUser.Password)) ? existingSystemUser.Password : BCrypt.Net.BCrypt.HashPassword(objSystemUser.Password);
                                objSystemUser.CreatedDate = existingSystemUser.CreatedDate;
                                objSystemUser.CreatedBy = existingSystemUser.CreatedBy;
                                objSystemUser.UpdatedDate = DateTime.Now;
                                objSystemUser.UpdatedBy = requestMessage.UserID;
                                _crmDbContext.SystemUser.Update(objSystemUser);

                            }
                        }
                        else
                        {
                            objSystemUser.Status = (int)Enums.Status.Active;
                            objSystemUser.CreatedDate = DateTime.Now;
                            objSystemUser.CreatedBy = requestMessage.UserID;
                            objSystemUser.Password = BCrypt.Net.BCrypt.HashPassword(objSystemUser.Password);
                            await _crmDbContext.SystemUser.AddAsync(objSystemUser);

                        }

                        await _crmDbContext.SaveChangesAsync();

                        // save departments which are mapped with this user
                        if (objSystemUser.lstDepartment.Count > 0)
                        {

                            List<SystemUserDepartmentMapping> lstSystemUserDepartmentMapping = await _crmDbContext.SystemUserDepartmentMapping.AsNoTracking()
                                .Where(x => x.SystemUserID == objSystemUser.SystemUserID).ToListAsync();

                            _crmDbContext.SystemUserDepartmentMapping.RemoveRange(lstSystemUserDepartmentMapping);


                            foreach (Department department in objSystemUser.lstDepartment)
                            {
                                SystemUserDepartmentMapping objSystemUserDepartmentMapping = new SystemUserDepartmentMapping();
                                objSystemUserDepartmentMapping.SystemUserID = objSystemUser.SystemUserID;
                                objSystemUserDepartmentMapping.DepartmentID = department.DepartmentID;
                                objSystemUserDepartmentMapping.DepartmentTypeID = department.DepartmentTypeID;

                                await _crmDbContext.SystemUserDepartmentMapping.AddAsync(objSystemUserDepartmentMapping);
                            }
                        }

                        // save organizations which are mapped with this user
                        if (objSystemUser.lstOrganization.Count > 0)
                        {

                            List<SystemUserOrganizationMapping> lstSystemUserOrganizationMapping = await _crmDbContext.SystemUserOrganizationMapping.AsNoTracking()
                                .Where(x => x.SystemUserID == objSystemUser.SystemUserID).ToListAsync();

                            _crmDbContext.SystemUserOrganizationMapping.RemoveRange(lstSystemUserOrganizationMapping);


                            foreach (Organization objOrganization in objSystemUser.lstOrganization)
                            {
                                SystemUserOrganizationMapping objSystemUserOrganizationMapping = new SystemUserOrganizationMapping();
                                objSystemUserOrganizationMapping.SystemUserID = objSystemUser.SystemUserID;
                                objSystemUserOrganizationMapping.OrganizationID = objOrganization.OrganizationID;

                                await _crmDbContext.SystemUserOrganizationMapping.AddAsync(objSystemUserOrganizationMapping);
                            }
                        }

                        await _crmDbContext.SaveChangesAsync();

                        objSystemUser.Password = string.Empty;
                        responseMessage.ResponseObj = objSystemUser;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "SaveSystemUser");
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveSystemUser");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }

            return responseMessage;
        }

        /// <summary>
        /// method for get system user by role.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetSystemUsersByRole(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<SystemUser> lstSystemUser = new List<SystemUser>();
                int role = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstSystemUser = await _crmDbContext.SystemUser.Where(x => x.Role == role && x.Status == (int)Enums.Status.Active)
                                        .OrderBy(x => x.SystemUserID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();

                foreach (SystemUser user in lstSystemUser)
                {
                    user.Password = string.Empty;
                }
                responseMessage.ResponseObj = lstSystemUser;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetSystemUserByRole");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetSystemUserByRole");
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
        public async Task<ResponseMessage> DeleteSystemUser(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Delete;

            try
            {
                SystemUser objSystemUser = JsonConvert.DeserializeObject<SystemUser>(requestMessage?.RequestObj.ToString());

                if (objSystemUser != null)
                {
                    SystemUser existingSystemUser = await _crmDbContext.SystemUser.AsNoTracking().FirstOrDefaultAsync(x => x.SystemUserID == objSystemUser.SystemUserID);

                    if (existingSystemUser?.SystemUserID > 0)
                    {
                        existingSystemUser.Status = (int)Enums.Status.Delete;
                        existingSystemUser.UpdatedBy = requestMessage.UserID;
                        existingSystemUser.UpdatedDate = DateTime.Now;
                        _crmDbContext.SystemUser.Update(existingSystemUser);
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "DeleteSystemUser");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }

            return responseMessage;
        }


       

        /// <summary>
        ///  get all system user by department id.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GetSystemUserByDepartmentId(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<VMUserAndDepartmentMapping> lstVMUserandDepartmentMapping = new List<VMUserAndDepartmentMapping>();

                List<VMUserAndShareFilterDepartment> lstVMUserAndShareFilterDepartment = new List<VMUserAndShareFilterDepartment>();

                List<int> lstDepartmentID = JsonConvert.DeserializeObject<List<int>>(requestMessage?.RequestObj.ToString());

                var userandDepartmentMapping = _crmDbContext.VMUserAndDepartmentMapping.Where(x => lstDepartmentID.Contains(x.DepartmentID)).AsQueryable();

                var gruobyDepartment = userandDepartmentMapping.ToList().GroupBy(x => x.SystemUserID);



                lstVMUserAndShareFilterDepartment= gruobyDepartment.Select(x=> new VMUserAndShareFilterDepartment()
                {
                    SystemUserID=x.FirstOrDefault().SystemUserID,
                    FullName=x.FirstOrDefault().FullName,
                    lstShareFilterDepartment=x.Select(df=> new ShareFilterDepartment()
                    {
                      DepartmentID=df.DepartmentID,
                      DepartmentName=df.DepartmentName
                    }).ToList()
                }).ToList();






                //lstVMUserandDepartmentMapping = userandDepartmentMapping.ToList();
                //responseMessage.TotalCount = lstVMUserandDepartmentMapping.Count;
                responseMessage.TotalCount = gruobyDepartment.Count();
                responseMessage.ResponseObj = lstVMUserAndShareFilterDepartment;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, " GetSystemUserByDepartmentId");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), " GetSystemUserByDepartmentId");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }


        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objSystemUser"></param>
        /// <returns></returns>
        private bool CheckedValidation(SystemUser objSystemUser, ResponseMessage responseMessage)
        {

            bool result = true;
            SystemUser existingSystemUser = new SystemUser();

            existingSystemUser = _crmDbContext.SystemUser.Where(x => x.UserName == objSystemUser.UserName && x.SystemUserID != objSystemUser.SystemUserID && x.Status == (int)Enums.Status.Active).AsNoTracking().FirstOrDefault();
            if (existingSystemUser != null)
            {
                responseMessage.Message = MessageConstant.DuplicateUserName;
                return false;
            }
            existingSystemUser = this._crmDbContext.SystemUser.Where(x => x.Email == objSystemUser.Email && x.SystemUserID != objSystemUser.SystemUserID && x.Status == (int)(Enums.Status.Active)).AsNoTracking().FirstOrDefault();
            if (existingSystemUser != null)
            {
                responseMessage.Message = MessageConstant.EmailAlreadyExist;
                return false;
            }
            existingSystemUser = this._crmDbContext.SystemUser.Where(x => x.PhoneNumber == objSystemUser.PhoneNumber && x.SystemUserID != objSystemUser.SystemUserID && x.Status == (int)(Enums.Status.Active)).AsNoTracking().FirstOrDefault();
            if (existingSystemUser != null)
            {
                responseMessage.Message = MessageConstant.PhoneNumberAlreadyExist;
                return false;
            }

            if (!String.IsNullOrEmpty(objSystemUser.Password) && (objSystemUser.Password != objSystemUser.ConfirmPassword))
            {
                responseMessage.Message = MessageConstant.ConfirmPasswordNotMatch;
                return false;
            }

            return true;
        }

    }


}
