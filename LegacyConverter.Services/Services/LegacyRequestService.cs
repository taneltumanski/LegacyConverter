using LegacyConverter.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LegacyConverter.Services.Services
{
	public class LegacyRequestService : ILegacyRequestService
	{
		public Task<string> Request(string apiEndpoint, string dataFile)
		{
			var address = new Uri(new Uri(apiEndpoint), dataFile);

			using (var wc = new WebClient()) {
				return wc.DownloadStringTaskAsync(address);
			}
		}
	}
}
