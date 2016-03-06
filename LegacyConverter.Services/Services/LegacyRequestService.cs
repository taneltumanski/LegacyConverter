using LegacyConverter.Core;
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
		public IConfig Config { get; set; }

		public string Request(string fileName)
		{
			var address = new Uri(new Uri(Config.ApiData.ApiEndpoint), fileName);

			using (var wc = new WebClient()) {
				try {
					return wc.DownloadString(address);
				} catch (WebException e ) {
					var response = e.Response as HttpWebResponse;

					if (response.StatusCode == HttpStatusCode.NotFound) {
						return null;
					}

					throw;
				}
			}
		}
	}
}
