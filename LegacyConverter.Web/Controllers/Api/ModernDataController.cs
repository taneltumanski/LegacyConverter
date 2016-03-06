using LegacyConverter.Core;
using LegacyConverter.Core.Dto;
using LegacyConverter.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace LegacyConverter.Web.Controllers.Api
{
	/// <summary>
	/// Simple service that gets periodical data from the legacy service
	/// </summary>
    public class ModernDataController : ApiController
    {
		public IDataItemService DataItemService { get; set; }

		/// <summary>
		/// Gets the currently buffered legacy data 
		/// </summary>
		/// <returns>Legacy data</returns>
		public DataItemDto Get()
		{
			return DataItemService.GetCurrentDataItem();
		}

		/// <summary>
		/// Gets a legacy data object based on the sequence id
		/// </summary>
		/// <param name="id">Legacy object id</param>
		/// <returns>Legacy object</returns>
		public DataItemDto Get(int id)
		{
			return DataItemService.RequestDataItem(id);
		}
	}
}
