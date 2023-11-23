using CRM.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services.Interface
{
	/// <summary>
	/// Interface
	/// </summary>
	public interface IDepartmentService
	{
		Task<ResponseMessage> GetAllDepartment(RequestMessage requestMessage);
		Task<ResponseMessage> SaveDepartment(RequestMessage requestMessage);
		Task<ResponseMessage> GetDepartmentById(RequestMessage requestMessage);

	}
}
