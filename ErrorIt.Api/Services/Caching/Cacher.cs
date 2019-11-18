using ErrorIt.Api.Extensions;
using ErrorIt.Api.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace ErrorIt.Api.Services.Caching
{
	public class Cacher : ICacher
	{
		private readonly IDistributedCache _distributedCache;

		public Cacher(IDistributedCache distributedCache)
		{
			_distributedCache = distributedCache;
		}

		public async Task<T> Get<T>(string key)
		{
			return (await _distributedCache.GetStringAsync($"{DotNetEnv.Env.GetString("cachebasename")}:{key}")).DeserializeJson<T>();
		}

		public async void Set(string key, object val, TimeSpan? expires)
		{
			await _distributedCache.SetStringAsync($"{DotNetEnv.Env.GetString("cachebasename")}:{key}", val.SerializeJson(), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expires });
		}

		public async void Remove(string key)
		{
			await _distributedCache.RemoveAsync($"{DotNetEnv.Env.GetString("cachebasename")}:{key}");
		}
	}
}
