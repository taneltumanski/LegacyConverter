using LegacyConverter.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyConverter.Core.Interfaces.Services
{
	/// <summary>
	/// Main data service wrapper for the legacy service
	/// </summary>
	public interface IDataItemService : IApplicationService
	{
		/// <summary>
		/// Updates and gets the current data item 
		/// </summary>
		/// <returns>Currently buffered data item</returns>
		DataItemDto GetCurrentDataItem();

		/// <summary>
		/// Requests the data item from the legacy service
		/// </summary>
		/// <param name="fileIndex">Data file index</param>
		/// <returns>Legacy data item</returns>
		DataItemDto RequestDataItem(int fileIndex);
	}
}
