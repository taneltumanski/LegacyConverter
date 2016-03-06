using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyConverter.Core.Interfaces.Services
{
	/// <summary>
	/// Gets the legacy data as string
	/// </summary>
	public interface ILegacyRequestService : IApplicationService
	{
		/// <summary>
		/// Makes a request to the legacy service and gets the data
		/// </summary>
		/// <param name="fileName">Legacy data filename</param>
		/// <returns>Legacy data as string</returns>
		string Request(string fileName);
	}
}
