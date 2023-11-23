using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/Expense")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService; 
        private readonly IExpenseTypeService _expenseTypeService;

        public ExpenseController(IExpenseService expenseService, IExpenseTypeService expenseTypeService)
        {
            this._expenseService = expenseService;
            this._expenseTypeService = expenseTypeService;
        }

        [HttpPost("GetAllExpense")]
        public async Task<ResponseMessage> GetAllExpense(RequestMessage requestMessage)
        {
            return await _expenseService.GetAllExpense(requestMessage);
        }

        [HttpPost("GetAllDailyExpenseByMonth")]
        public async Task<ResponseMessage> GetAllDailyExpenseByMonth(RequestMessage requestMessage)
        {
            return await _expenseService.GetAllDailyExpenseByMonth(requestMessage);
        }

        [HttpPost("GetAllMonthlyExpenseByMonth")]
        public async Task<ResponseMessage> GetAllMonthlyExpenseByMonth(RequestMessage requestMessage)
        {
            return await _expenseService.GetAllMonthlyExpenseByMonth(requestMessage);
        }

        [HttpPost("GetMonthlySummaryReport")]
        public async Task<ResponseMessage> GetMonthlySummaryReport(RequestMessage requestMessage)
        {
            return await _expenseService.GetMonthlySummaryReport(requestMessage);
        }

        [HttpPost("GeExpenseById")]
        public async Task<ResponseMessage> GetExpenseById(RequestMessage requestMessage)
        {
            return await this._expenseService.GetExpenseById(requestMessage);
        }

        [HttpPost("GetAllDailyExpense")]
        public async Task<ResponseMessage> GetAllDailyExpense(RequestMessage requestMessage)
        {
            return await _expenseService.GetAllDailyExpense(requestMessage);
        }

        [HttpPost("GetExpensesByDate")]
        public async Task<ResponseMessage> GetExpensesByDate(RequestMessage requestMessage)
        {
            return await _expenseService.GetExpenseByDate(requestMessage);
        }

        [HttpPost("SaveExpense")]
        public async Task<ResponseMessage> SaveExpense(RequestMessage requestMessage)
        {
            string apiRootDir = System.IO.Directory.GetCurrentDirectory();
            return await _expenseService.SaveExpense(requestMessage, apiRootDir);
        }

        [HttpPost("UpdateExpense")]
        public async Task<ResponseMessage> UpdateExpense(RequestMessage requestMessage)
        {
            return await _expenseService.UpdateExpense(requestMessage);
        }

        [HttpPost("DeleteExpense")]
        public async Task<ResponseMessage> DeleteExpense(RequestMessage requestMessage)
        {
            return await _expenseService.DeleteExpense(requestMessage);
        }

        #region "Expense Type apis"

        [HttpPost("getAllExpenseTypeByExpenseCategoryID")]
        public async Task<ResponseMessage> getAllExpenseTypeByExpenseCategoryID(RequestMessage requestMessage)
        {
            return await _expenseTypeService.GetExpenseTypeByExpenseCategoryId(requestMessage);
        }

        [HttpPost("GetAttachment")]
        public async Task<ResponseMessage> GetAttachment(RequestMessage requestMessage)
        {
            return await _expenseService.GetAttachment(requestMessage);
        }

        [HttpGet("gettest")]
        public string gettest()
        {
            return "Pass..";
        }




        #endregion

    }
}
