using CRM.Common.Constants;
using CRM.Common.DTO;
using CRM.Common.Enums;
using CRM.Common.Helper;
using CRM.Common.Models;
using CRM.DataAccess;
using CRM.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CRM.Services
{
#pragma warning disable CS8600
    public class EmployeeOIdService : IEmployeeService
    {
        private readonly CRMDbContext _crmDbContext;

        public EmployeeOIdService(CRMDbContext ctx)
        {
            this._crmDbContext = ctx;
        }

        /// <summary>
        /// Get all employee
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllEmployee(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<EmployeeOld> lstEmployee = new List<EmployeeOld>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstEmployee = await _crmDbContext.Employee.OrderBy(x => x.EmployeeID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstEmployee;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllEmployee");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllEmployee");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Get employee by id.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GetEmployeeById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                EmployeeOld objEmployeeOld = new EmployeeOld();
                int employeeID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objEmployeeOld = await _crmDbContext.Employee.FirstOrDefaultAsync(x => x.EmployeeID == employeeID && x.Status == (int)Enums.Status.Active);
                responseMessage.ResponseObj = objEmployeeOld;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetEmployeeById");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetEmployeeById");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Save and update employee.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> SaveEmployee(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {

                EmployeeOld objEmployeeOld = JsonConvert.DeserializeObject<EmployeeOld>(requestMessage?.RequestObj.ToString());

                if (objEmployeeOld != null)
                {
                    if (CheckedValidation(objEmployeeOld, responseMessage))
                    {
                        if (objEmployeeOld.EmployeeID > 0)
                        {
                            EmployeeOld existingEmployeeOld = await this._crmDbContext.Employee.AsNoTracking().FirstOrDefaultAsync(x => x.EmployeeID == objEmployeeOld.EmployeeID && x.Status == (int)Enums.Status.Active);
                            if (existingEmployeeOld != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objEmployeeOld.CreatedDate = existingEmployeeOld.CreatedDate;
                                objEmployeeOld.CreatedBy = existingEmployeeOld.CreatedBy;
                                objEmployeeOld.UpdatedDate = DateTime.Now;
                                objEmployeeOld.UpdatedBy = requestMessage.UserID;
                                _crmDbContext.Employee.Update(objEmployeeOld);
                            }
                        }
                        else
                        {
                            objEmployeeOld.Status = (int)Enums.Status.Active;
                            objEmployeeOld.CreatedDate = DateTime.Now;
                            objEmployeeOld.CreatedBy = requestMessage.UserID;
                            await _crmDbContext.Employee.AddAsync(objEmployeeOld);

                        }

                        await _crmDbContext.SaveChangesAsync();

                        responseMessage.ResponseObj = objEmployeeOld;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "SaveEmployee");
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveEmployee");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }


            return responseMessage;
        }
        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objEmployeeOld"></param>
        /// <returns></returns>
        private bool CheckedValidation(EmployeeOld objEmployeeOld, ResponseMessage responseMessage)
        {
            if (string.IsNullOrEmpty(objEmployeeOld.EmployeeName))
            {
                responseMessage.Message = MessageConstant.EmployeeName;
                return false;
            }
            if (string.IsNullOrEmpty(objEmployeeOld.Email))
            {
                responseMessage.Message = MessageConstant.Email;
                return false;
            }

            return true;
        }
#pragma warning restore CS8600

    }

}
