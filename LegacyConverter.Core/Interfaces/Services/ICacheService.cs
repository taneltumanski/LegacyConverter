using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyConverter.Core.Interfaces.Services
{
	public interface ICacheService : IApplicationService
	{
		void Add(object item, string key);
		void Add(object item, string key, DateTimeOffset absoluteExpiration);
		void Add(object item, string key, TimeSpan slidingExpiration);
		
		T Get<T>(string key) where T : class;
	}
}
