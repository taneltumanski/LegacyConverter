using LegacyConverter.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace LegacyConverter.Services.Services
{
	public class CacheService : ICacheService
	{
		private MemoryCache Cache = MemoryCache.Default;

		public void Add(object item, string key)
		{
			Add(item, key, null, null);
		}

		public void Add(object item, string key, TimeSpan slidingExpiration)
		{
			Add(item, key, slidingExpiration, null);
		}

		public void Add(object item, string key, DateTimeOffset absoluteExpiration)
		{
			Add(item, key, null, absoluteExpiration);
		}

		private void Add(object item, string key, TimeSpan? slidingExpiration, DateTimeOffset? absoluteExpiration)
		{
			Cache.Add(key, item, new CacheItemPolicy() {
				AbsoluteExpiration = absoluteExpiration != null ? absoluteExpiration.Value : ObjectCache.InfiniteAbsoluteExpiration,
				SlidingExpiration = slidingExpiration != null ? slidingExpiration.Value : ObjectCache.NoSlidingExpiration
			});
		}

		public T Get<T>(string key) where T : class
		{
			return MemoryCache.Default.Get(key) as T;
		}
	}
}
