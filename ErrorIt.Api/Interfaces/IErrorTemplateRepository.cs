using System.Collections.Generic;
using System.Threading.Tasks;
using ErrorIt.Api.Data.Models;
using ErrorIt.Api.Models;

namespace ErrorIt.Api.Interfaces
{ 
	public interface IErrorTemplateRepository
	{
		Task<ErrorTemplate> Create(int applicationId, ErrorResponse errorResponse);
		Task<bool> Delete(int applicationId, int id);
		Task<List<ErrorTemplate>> Get(int applicationId);
		Task<ErrorTemplate> Get(int applicationId, string applicationErrorCode);
		Task<ErrorTemplate> GetById(int id);
		Task<ErrorTemplate> Update(int applicationId, int id, ErrorResponse errorResponse);
	}
}