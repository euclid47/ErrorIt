using System.Collections.Generic;
using System.Threading.Tasks;
using ErrorIt.Api.Data.Models;

namespace ErrorIt.Api.Interfaces
{ 
	public interface IApplicationRepository
	{
		Task<Application> Create(int applicationGroupId, string name, string description);
		Task<List<Application>> Get(int applicationGroupId);
		Task<Application> Get(int applicationGroupId, int id);
		Task<Application> Get(int applicationGroupId, string name);
		Task<Application> Update(int applicationGroupId, int id, string name, string description);
		Task<bool> Delete(int applicationId, int id);
		Task<bool> Exists(int applicationGroupId, int id);
	}
}