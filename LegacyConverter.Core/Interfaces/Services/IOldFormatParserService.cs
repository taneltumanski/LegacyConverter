using LegacyConverter.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyConverter.Core.Interfaces.Services
{
	/// <summary>
	/// Converts the legacy string data to an object
	/// </summary>
	public interface IOldFormatParserService : IApplicationService
	{
		/// <summary>
		/// Converts the legacy string data to an object
		/// </summary>
		/// <param name="format">Legacy data format</param>
		/// <returns>Modern object for legacy data</returns>
		DataItemDto ParseOldFormat(string format);
	}
}
