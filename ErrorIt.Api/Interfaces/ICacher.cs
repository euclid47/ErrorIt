using System;
using System.Threading.Tasks;

namespace ErrorIt.Api.Interfaces
{
	public interface ICacher
	{
		Task<T> Get<T>(string key);
		void Remove(string key);
		void Set(string key, object val, TimeSpan? expires);
	}
}