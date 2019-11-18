using System.Collections.Generic;
using System.Threading.Tasks;
using ErrorIt.Api.Data.Models;

namespace ErrorIt.Api.Interfaces
{
	public interface IApplicationGroupRepository
	{
		Task<ApplicationGroup> Create(string name, string description);
		Task<bool> Delete(int id);
		Task<List<ApplicationGroup>> Get();
		Task<ApplicationGroup> Get(int id);
		Task<ApplicationGroup> Get(string name);
		Task<ApplicationGroup> Update(int id, string name, string description);
	}
}