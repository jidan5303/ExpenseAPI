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
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly CRMDbContext _context;
        public LeaveRequestService(CRMDbContext context) { 
            _context = context;
        }

        public async Task<ResponseMessage> DeleteLeaveRequest(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                var ID = JsonConvert.DeserializeObject<int>(requestMessage.RequestObj.ToString());
                LeaveRequest exist  = _context.LeaveRequest.Where(l => l.LeaveRequestID == ID).FirstOrDefault();
                exist.Status = (int)Enums.Status.Delete;
                _context.LeaveRequest.Update(exist);
                await _context.SaveChangesAsync();

                responseMessage.Message = MessageConstant.DeleteSuccess;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllExpense");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        public async Task<ResponseMessage> AcceptLeaveRequest(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                var ID = JsonConvert.DeserializeObject<int>(requestMessage.RequestObj.ToString());
                LeaveRequest exist = _context.LeaveRequest.Where(l => l.LeaveRequestID == ID).FirstOrDefault();
                exist.LeaveStatus = "Accepted";
                _context.LeaveRequest.Update(exist);
                await _context.SaveChangesAsync();

                responseMessage.Message = MessageConstant.SavedSuccessfully;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllExpense");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        public async Task<ResponseMessage> RejectLeaveRequest(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                var leave = JsonConvert.DeserializeObject<LeaveRequest>(requestMessage.RequestObj.ToString());
                LeaveRequest exist = _context.LeaveRequest.Where(l => l.LeaveRequestID == leave.LeaveRequestID).FirstOrDefault();
                exist.LeaveStatus = "Rejected";
                exist.RejectionCause = leave.RejectionCause;
                _context.LeaveRequest.Update(exist);
                await _context.SaveChangesAsync();

                responseMessage.Message = MessageConstant.SavedSuccessfully;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllExpense");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        public async Task<ResponseMessage> GetAllLeaveRequest(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                var currentYear = DateTime.Now.Year;
                List<LeaveRequest> lstLeaveRequest = await _context.LeaveRequest
                    .Where(x => x.Status == (int)Enums.Status.Active && x.StartDate.Year == currentYear && x.EndDate.Year == currentYear)
                    .OrderByDescending(x=>x.LeaveRequestID).ToListAsync();
                foreach (var leave in lstLeaveRequest)
                {
                    leave.LeaveEmployee = await _context.LeaveEmployee.Where(x => x.EmployeeID == leave.EmployeeID).FirstOrDefaultAsync();
                    leave.LeaveType = await _context.LeaveType.Where(x => x.LeaveTypeID ==  leave.LeaveTypeID).FirstOrDefaultAsync();
                }
                
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                responseMessage.ResponseObj = lstLeaveRequest;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllExpense");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        public async Task<ResponseMessage> GetAllLeaveRequestByID(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                var ID = JsonConvert.DeserializeObject<int>(requestMessage.RequestObj.ToString());
                var currentYear = DateTime.Now.Year;
                List<LeaveRequest> lstLeaveRequest = await _context.LeaveRequest
                    .Where(x => x.EmployeeID == ID && x.Status == (int)Enums.Status.Active && x.StartDate.Year == currentYear && x.EndDate.Year == currentYear)
                    .OrderByDescending(x => x.LeaveRequestID).ToListAsync();
                foreach (var leave in lstLeaveRequest)
                {
                    leave.LeaveEmployee = await _context.LeaveEmployee.Where(x => x.EmployeeID == leave.EmployeeID).FirstOrDefaultAsync();
                    leave.LeaveType = await _context.LeaveType.Where(x => x.LeaveTypeID == leave.LeaveTypeID).FirstOrDefaultAsync();
                }

                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                responseMessage.ResponseObj = lstLeaveRequest;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllExpense");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        public async Task<ResponseMessage> GetAllLeaveRequestByYear(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                var currentYear = JsonConvert.DeserializeObject<int>(requestMessage.RequestObj.ToString());
                List<LeaveRequest> lstLeaveRequest = await _context.LeaveRequest
                    .Where(x => x.Status == (int)Enums.Status.Active && x.StartDate.Year == currentYear && x.EndDate.Year == currentYear)
                    .OrderByDescending(x => x.LeaveRequestID).ToListAsync();
                foreach (var leave in lstLeaveRequest)
                {
                    leave.LeaveEmployee = await _context.LeaveEmployee.Where(x => x.EmployeeID == leave.EmployeeID).FirstOrDefaultAsync();
                    leave.LeaveType = await _context.LeaveType.Where(x => x.LeaveTypeID == leave.LeaveTypeID).FirstOrDefaultAsync();
                }

                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                responseMessage.ResponseObj = lstLeaveRequest;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllExpense");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        public async Task<ResponseMessage> SaveLeaveRequest(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                LeaveRequest objLeaveRequest = JsonConvert.DeserializeObject<LeaveRequest>(requestMessage.RequestObj.ToString());
                if(objLeaveRequest.LeaveRequestID > 0)
                {
                    LeaveRequest exist = _context.LeaveRequest.Where(x => x.LeaveRequestID == objLeaveRequest.LeaveRequestID).AsNoTracking().FirstOrDefault();
                    objLeaveRequest.Status = exist.Status;
                    objLeaveRequest.LeaveStatus = exist.LeaveStatus;
                    objLeaveRequest.RequestDate = exist.RequestDate;

                    _context.LeaveRequest.Update(objLeaveRequest);
                }
                else
                {
                    objLeaveRequest.RequestDate = DateTime.Now;
                    objLeaveRequest.LeaveStatus = "Pending";
                    objLeaveRequest.Status = (int)Enums.Status.Active;
                    _context.LeaveRequest.Add(objLeaveRequest);
                }
                await _context.SaveChangesAsync();
                responseMessage.Message = MessageConstant.SavedSuccessfully;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                responseMessage.ResponseObj = objLeaveRequest;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllExpense");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }
        
        public async Task<ResponseMessage> GetAllLeaveType(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<LeaveType> lstLeaveType = await _context.LeaveType.ToListAsync();

                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                responseMessage.ResponseObj = lstLeaveType;
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
