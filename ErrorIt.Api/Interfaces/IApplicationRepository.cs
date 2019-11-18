using System.Collections.Generic;
using System.Threading.Tasks;
using ErrorIt.Api.Data.Models;

namespace ErrorIt.Api.Interfaces
{ 
	public interface IApplicationRepository
	{
		Task<Application> Create(int applicationGroupId, string name, string description);
		Task<List<Application>> Get(int applicationGroupId);
		Task<Application> GetById(int id);
		Task<Application> Get(string name);
		Task<Application> Update(int applicationGroupId, int id, string name, string description);
		Task<bool> Delete(int applicationId, int id);
	}
}