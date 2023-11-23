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
	public interface IOrganizationService
	{
		Task<ResponseMessage> GetAllOrganization(RequestMessage requestMessage);
		Task<ResponseMessage> SaveOrganization(RequestMessage requestMessage);
		Task<ResponseMessage> GetOrganizationById(RequestMessage requestMessage);

	}
}
