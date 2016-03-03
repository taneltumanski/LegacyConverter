using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyConverter.Core.Interfaces.Services
{
	public interface ILegacyRequestService : IApplicationService
	{
		Task<string> Request(string apiEndpoint, string dataFile);
	}
}
