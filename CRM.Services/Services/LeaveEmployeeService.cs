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

namespace CRM.Services.Services
{
    public class LeaveEmployeeService : ILeaveEmployeeService
    {
        private readonly CRMDbContext _crmDbContext;
        public LeaveEmployeeService(CRMDbContext ctx)
        {
            _crmDbContext = ctx;
        }
        public async Task<ResponseMessage> DeleteEmployee(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                int objLeaveEmployeeID = JsonConvert.DeserializeObject<int>(requestMessage.RequestObj.ToString());

                LeaveEmployee exist = _crmDbContext.LeaveEmployee.Where(x => x.EmployeeID == objLeaveEmployeeID).FirstOrDefault();
                exist.Status = (int)Enums.Status.Delete;

                _crmDbContext.LeaveEmployee.Update(exist);

                await _crmDbContext.SaveChangesAsync();
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                responseMessage.Message = MessageConstant.DeleteSuccess;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllExpense");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        public async Task<ResponseMessage> GetEmployee(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<LeaveEmployee> lstEmployee = new List<LeaveEmployee>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstEmployee = await _crmDbContext.LeaveEmployee.Where(x => x.Status == (int)Enums.Status.Active).OrderBy(x => x.EmployeeID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstEmployee;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllExpense");
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllExpense");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        public async Task<ResponseMessage> SaveEmployee(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                LeaveEmployee objLeaveEmployee = JsonConvert.DeserializeObject<LeaveEmployee>(requestMessage.RequestObj.ToString());
                if (objLeaveEmployee.EmployeeID > 0)
                {
                    LeaveEmployee exist = _crmDbContext.LeaveEmployee.Where(x => x.EmployeeID == objLeaveEmployee.EmployeeID).AsNoTracking().FirstOrDefault();
                    _crmDbContext.LeaveEmployee.Update(objLeaveEmployee);
                }
                else
                {
                    objLeaveEmployee.Status = (int)Enums.Status.Active;
                    await _crmDbContext.LeaveEmployee.AddAsync(objLeaveEmployee);
                }
                await _crmDbContext.SaveChangesAsync();
                responseMessage.ResponseObj = objLeaveEmployee;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                responseMessage.Message = MessageConstant.SavedSuccessfully;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllExpense");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        public async Task<ResponseMessage> GetLeaveBalance(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                var employeeId = JsonConvert.DeserializeObject<int>(requestMessage.RequestObj.ToString());
                var leaveTypes = _crmDbContext.LeaveType.ToList();
                var employee = _crmDbContext.LeaveEmployee.Where(x => x.EmployeeID == employeeId).FirstOrDefault();
                var leaveDurations = _crmDbContext.LeaveDuration.ToList();
                var currentYear = DateTime.Now.Year;
                var leaveRequests = _crmDbContext.LeaveRequest
                                    .Where(lr => lr.EmployeeID == employeeId &&
                                                 lr.LeaveStatus == "Accepted" &&
                                                 lr.StartDate.Year == currentYear && lr.EndDate.Year == currentYear)
                                    .ToList();

                var report = leaveTypes.Select(leaveType =>
                {
                    var allowance = leaveDurations.FirstOrDefault(ld => ld.LeaveTypeID == leaveType.LeaveTypeID)?.Duration ?? 0;
                    var used = leaveRequests.Where(lr => lr.LeaveTypeID == leaveType.LeaveTypeID).Sum(lr => lr.Duration);
                    var available = allowance - used;

                    return new
                    {
                        LeaveTypeName = leaveType.LeaveTypeName,
                        Allowance = allowance,
                        Used = used,
                        Available = available
                    };
                }).ToList();

                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                responseMessage.ResponseObj = report;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllExpense");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }
    }
}
